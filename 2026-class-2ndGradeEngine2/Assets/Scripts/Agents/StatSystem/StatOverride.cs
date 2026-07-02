using System;
using UnityEngine;

namespace Agents.StatSystem
{
    [Serializable]
    public class StatOverride
    {
        [field: SerializeField] public StatSO StatData { get; private set; }
        [SerializeField] private bool isUseOverride;
        [SerializeField] private float overrideValue;

        public StatOverride(StatSO originalStat) => this.StatData = originalStat;

        public StatSO CreateStat()
        {
            Debug.Assert(StatData != null, "Stat Data is null");

            StatSO newStat = StatData.Clone() as StatSO;
            Debug.Assert(newStat != null, $"New Stat Data is null : {StatData.StatName}");

            if (isUseOverride)
                newStat.BaseValue = overrideValue;
            return newStat;
        }
    }
}