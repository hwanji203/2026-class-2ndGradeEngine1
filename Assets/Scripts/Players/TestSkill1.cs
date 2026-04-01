using DG.Tweening;
using System;
using UnityEngine;

namespace Test.TestSkillSystem
{
    public class TestSkill1 : MonoBehaviour, ITestSkill
    {
        public event Action OnSkillEnd;

        [SerializeField] private float cooldonwDuration = 2f;
        private float _lastUseTime;

        public bool IsUsing { get; private set; }

        [field: SerializeField] public int SkillIndex { get; private set; }

        public bool CanAttack()
        {
            return _lastUseTime + cooldonwDuration < Time.time && !IsUsing;
        }

        public void Attack()
        {
            Debug.Log("테스트1 공격 시작");
            IsUsing = true;
            DOVirtual.DelayedCall(1f, EndSkill);
        }

        private void EndSkill()
        {
            _lastUseTime = Time.time;
            IsUsing = false;
            Debug.Log("테스트1 <color=red>종료</color>");
            OnSkillEnd?.Invoke();
        }
    }

}

