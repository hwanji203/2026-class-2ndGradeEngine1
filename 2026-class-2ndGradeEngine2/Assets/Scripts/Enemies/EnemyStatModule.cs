using Agents;
using Agents.StatSystem;
using CombatSystem;
using GGMLib.ModuleSystem;
using UnityEngine;

namespace Enemies
{
    public class EnemyStatModule : AbstractAgentStatModule
    {
        [Header("Damage related stat list")]
        [SerializeField] private StatSO strStat;
        [SerializeField] private StatSO intStat;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            if (!TryGetStat(strStat.AssetIndex, out strStat))
                Debug.LogError("힘 스탯이 없습니다. 확인하세요.");
            if (!TryGetStat(intStat.AssetIndex, out strStat))
                Debug.LogError("지능 스탯이 없습니다. 확인하세요.");
        }

        public override DamageData CalculateDamage(SkillDataSO skillData)
        {
            float damage = skillData.damageType switch
            {
                SkillDamageType.Physical => (strStat.Value + skillData.baseDamage) * skillData.damageMultiplier,
                SkillDamageType.Magical => (intStat.Value + skillData.baseDamage) * skillData.damageMultiplier,
                _ => skillData.baseDamage
            };

            return new DamageData(damage, Vector3.zero, Vector3.zero, _owner, false);
        }
    }
}