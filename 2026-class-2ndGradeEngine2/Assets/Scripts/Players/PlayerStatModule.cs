using Agents;
using Agents.StatSystem;
using CombatSystem;
using GGMLib.ModuleSystem;
using UnityEngine;

namespace Players
{
    public class PlayerStatModule : AbstractAgentStatModule
    {
        [Header("Damage related stat list")]
        [SerializeField] private StatSO strStat;

        [SerializeField] private StatSO intStat;
        [SerializeField] private StatSO criticalStat;
        [SerializeField] private StatSO criticalDamageStat;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            if (!TryGetStat(strStat.AssetIndex, out strStat))
                Debug.LogError("힘 스탯이 없습니다. 확인하세요.");
            if (!TryGetStat(intStat.AssetIndex, out strStat))
                Debug.LogError("지능 스탯이 없습니다. 확인하세요.");
            if (!TryGetStat(criticalStat.AssetIndex, out strStat))
                Debug.LogError("크리티컬 스탯이 없습니다. 확인하세요.");
            if (!TryGetStat(criticalDamageStat.AssetIndex, out strStat))
                Debug.LogError("크리티컬 데미지 스탯이 없습니다. 확인하세요.");
        }

        public override DamageData CalculateDamage(SkillDataSO skillData)
        {
            float damage = skillData.damageType switch
            {
                SkillDamageType.Physical => (strStat.Value + skillData.baseDamage) * skillData.damageMultiplier,
                SkillDamageType.Magical => (intStat.Value + skillData.baseDamage) * skillData.damageMultiplier,
                _ => skillData.baseDamage
            };
            bool isCritical = Random.value < criticalStat.Value;
            if (isCritical)
            {
                damage *= criticalDamageStat.Value; // 증명
            }

            return new DamageData(damage, Vector3.zero, Vector3.zero, _owner, isCritical);
        }
    }
}