using System;
using System.Collections.Generic;
using System.Linq;
using Agents;
using Agents.StatSystem;
using CombatSystem;
using GGMLib.AnimationSystem;
using GGMLib.EventChannelSystem;
using GGMLib.ModuleSystem;
using Players.FSM;
using UnityEngine;

namespace Players
{
    public class PlayerSkillModule : MonoBehaviour, ISkillModule, IModule, IAfterInitModule
    {
        [field: SerializeField] public GameEventChannelSO CreateChannel { get; private set; }
        
        [SerializeField] private AnimParamSO attackSpeedParam;
        [SerializeField] private StatSO attackSpeedStat;
        
        public ModuleOwner Owner { get; private set; }
        public PlayerController Player { get; private set; }
        public event Action OnCurrentSkillEnd;

        private IStatModule _statModule;
        private IRenderer _renderer;
        
        private Dictionary<int, ISkill> _skillDict;
        private ISkill _currentSkill;
        private float _speedMultiplier;
        
        public void Initialize(ModuleOwner owner)
        {
            Owner = owner;
            Player = Owner as PlayerController;
            Debug.Assert(Player != null, "플레이어 스킬 모듈은 플레이어의 자식으로 있어야 합니다");

            _skillDict = GetComponentsInChildren<ISkill>()
                .ToDictionary(skill => skill.SkillData.skillIndex);

            foreach (ISkill skill in _skillDict.Values)
            {
                skill.InitializeSkill(this); //스킬들을 초기화
            }

            _statModule = Owner.GetModule<IStatModule>();
            Debug.Assert(_statModule != null, "StatModule은 플레이어 스킬에 필요합니다.");
            _renderer = Owner.GetModule<IRenderer>();
            Debug.Assert(_renderer != null, "Renderer는 플레이어 스킬에 필요합니다.");
        }

        public void AfterInit()
        {
            Player.PlayerInput.OnAttackKeyPressed += HandleAttackKeyPress; //기본공격은 이걸로 바인딩
            Player.PlayerInput.OnSlideKeyPressed += HandleSlideKeyPress;

            _speedMultiplier = _statModule.SubscribeStat(attackSpeedStat.AssetIndex, HandleAttackSpeed, 1f);
            _renderer.Animator.SetFloat(attackSpeedParam.ParamHash, _speedMultiplier);
        }

        private void OnDestroy()
        {
            if (Player != null && Player.PlayerInput != null)
            {
                Player.PlayerInput.OnAttackKeyPressed -= HandleAttackKeyPress; //기본공격은 이걸로 바인딩
                Player.PlayerInput.OnSlideKeyPressed -= HandleSlideKeyPress;                
            }
            
            _statModule?.UnSubscribeStat(attackSpeedStat.AssetIndex, HandleAttackSpeed);
        }

        private void HandleAttackSpeed(StatSO stat, float currentValue, float prevValue)
        {
            _speedMultiplier = currentValue;
            _renderer.Animator.SetFloat(attackSpeedParam.ParamHash, _speedMultiplier);
        }

        private void HandleSlideKeyPress()
        {
            if (CanUseSkill(1))
            {
                Player.ChangeState(PlayerState.SKILL, 0);
                UseSkill(1);
            }
        }

        private void HandleAttackKeyPress()
        {
            if (CanUseSkill(0))
            {
                Player.ChangeState(PlayerState.SKILL, 0); 
                //스킬상태 애니메이션 제어는 각 스킬 콤포넌트가 할거라서 duration 0으로 보냈다.
                UseSkill(0);
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
                //아직은 기존스킬 캔슬하고 쏘는건 안만든다.
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
            _currentSkill = null;
        }

        public void InvokeSkillEnd() => OnCurrentSkillEnd?.Invoke();
        
        public void StopSkillIfNotFinished()
        {
            if (_currentSkill != null)
            {
                _currentSkill.StopSkill();
                _currentSkill = null;
            }
        }
    }
}