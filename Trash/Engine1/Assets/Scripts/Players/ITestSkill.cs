using System;

namespace Test.TestSkillSystem
{
    public interface ITestSkill
    {
        public event Action OnSkillEnd;
        public int SkillIndex { get; }
        public bool IsUsing { get; }

        public bool CanAttack();
        public void Attack();
    }
}

