using CombatSystem;
using GGMLib.ModuleSystem;
using Players.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Players
{
    public class PlayerSkillModule : MonoBehaviour, ISkillModule, IModule
    {
        public ModuleOwner Owner { get; private set; }

        public PlayerController Player { get; private set; }
        public event Action OnCurrentSkillEnd;

        protected Dictionary<int, ISkill> _skillDict;
        private ISkill _currentSkill;

        public void Initialize(ModuleOwner owner)
        {
            Owner = owner;
            Player = Owner as PlayerController;
            Debug.Assert(Player != null, "�÷��̾� ��ų ����� �÷��̾��� �ڽ����� �־�� �մϴ�.");

            _skillDict = GetComponentsInChildren<ISkill>()
                .ToDictionary(skill => skill.SkillData.skillIndex);

            foreach (ISkill skill in _skillDict.Values)
            {
                skill.InitializeSkill(this);
            }

            Player.PlayerInput.OnAttackKeyPressed += HandleAttackKeyPress;
            Player.PlayerInput.OnSlideKeyPressed += HandleSlideKeyPress;
        }

        private void OnDestroy()
        {
            if (Player != null && Player.PlayerInput != null)
            {
                Player.PlayerInput.OnAttackKeyPressed -= HandleAttackKeyPress;
                Player.PlayerInput.OnSlideKeyPressed -= HandleSlideKeyPress;
            }
        }

        private void HandleAttackKeyPress()
        {
            if (CanUseSkill(0))
            {
                Player.ChangeState(PlayerState.SKILL, 0);
                UseSkill(0);
            }
        }

        private void HandleSlideKeyPress()
        {
            if (CanUseSkill(1))
            {
                Player.ChangeState(PlayerState.SKILL, 0);
                UseSkill(1);
            }
        }

        public bool CanUseSkill(int skillIndex, GameObject target = null)
        {
            if (_currentSkill is { IsUsing: true })
                return false;

            if (_skillDict.TryGetValue(skillIndex, out ISkill skill))
            {
                return skill.CanUseSkill(target);
            }

            return false;
        }

        public void UseSkill(int skillIndex, GameObject target = null)
        {
            if (_skillDict.TryGetValue(skillIndex, out ISkill skill))
            {
                if (_currentSkill != null)
                    _currentSkill.OnSkillEnd -= HandleCurrentSkillEnd;
                _currentSkill = skill;
                _currentSkill.OnSkillEnd += HandleCurrentSkillEnd;
                skill.UseSkill(target);
            }
        }

        private void HandleCurrentSkillEnd()
        {
            _currentSkill.OnSkillEnd -= HandleCurrentSkillEnd;
            InvokeSkillEnd();
        }

        public void InvokeSkillEnd() => OnCurrentSkillEnd?.Invoke();
    }
}

