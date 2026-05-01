using GGMLib.ModuleSystem;
using System;
using UnityEngine;

namespace CombatSystem
{
    public interface ISkillModule
    {
        ModuleOwner Owner { get; }

        event Action OnCurrentSkillEnd;
        bool CanUseSkill(int skillIndex, GameObject target = null);
        void UseSkill(int skillIndex, GameObject target = null);
        void InvokeSkillEnd();
    }

}
