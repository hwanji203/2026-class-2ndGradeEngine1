using GGMLib.ModuleSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Test.TestSkillSystem
{
    public class TestSkillModule : MonoBehaviour, IModule
    {
        private TestJumpAttack _jumpAttack;
        private TestRolling _rolling;

        private TestPlayer _player;
        private Dictionary<int, ITestSkill> _skillDict;

        public void Initialize(ModuleOwner owner)
        {
            _player = owner as TestPlayer;

            _skillDict = GetComponentsInChildren<ITestSkill>()
                .ToDictionary(skill => skill.SkillIndex);

            _jumpAttack = GetComponentInChildren<TestJumpAttack>();
            _rolling = GetComponentInChildren<TestRolling>();

            _player.PlayerInput.OnSkillKeyPressed += HandleSkillKeyPress;
        }

        private void OnDestroy()
        {
            _player.PlayerInput.OnSkillKeyPressed -= HandleSkillKeyPress;
        }

        private void HandleSkillKeyPress(int skillIndex)
        {
            if (_skillDict.TryGetValue(skillIndex, out var skill))
            {
                Debug.Log(skillIndex);
                Debug.Log(skill);
                if (skill.CanAttack())
                    skill.Attack();
            }
        }
    }
}