using System;
using UnityEngine;

namespace CombatSystem
{
    public interface ISkill
    {
        event Action OnSkillEnd;
        SkillDataSO SkillData { get; }
        bool IsUsing { get; }

        float NormalizedCooldown { get; }

        void InitializeSkill(ISkillModule skillModule);
        bool CanUseSkill(GameObject target = null);
        void UseSkill(GameObject target = null);
        void StopSkill();
    }
}

