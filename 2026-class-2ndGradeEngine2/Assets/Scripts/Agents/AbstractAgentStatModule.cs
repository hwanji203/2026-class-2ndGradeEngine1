using System.Collections.Generic;
using System.Linq;
using Agents.StatSystem;
using CombatSystem;
using GGMLib.ModuleSystem;
using UnityEngine;

namespace Agents
{
    public abstract class AbstractAgentStatModule : MonoBehaviour, IModule, IStatModule
    {
        [SerializeField] private StatOverride[] statOverrides;

        protected ModuleOwner _owner;
        protected Dictionary<int, StatSO> _statDict;
        
        public virtual void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            _statDict = statOverrides.ToDictionary(stat => stat.StatData.AssetIndex, stat => stat.CreateStat());
        }
        
        public StatSO GetStat(int statIndex) => _statDict.GetValueOrDefault(statIndex);

        public bool TryGetStat(int statIndex, out StatSO stat) => _statDict.TryGetValue(statIndex, out stat);

        public void AddModifier(int statIndex, object key, float value)
        {
            if (_statDict.TryGetValue(statIndex, out StatSO stat))
            {
                stat.AddModifier(key, value);
                return;
            }
            Debug.LogWarning($"수정 가능한 스탯이 존재하지 않습니다. 인덱스 번호 확인 : {statIndex}");
        }

        public void RemoveModifier(int statIndex, object key)
        {
            if (_statDict.TryGetValue(statIndex, out StatSO stat))
            {
                stat.RemoveModifier(key);
                return;
            }
            Debug.LogWarning($"수정 가능한 스탯이 존재하지 않습니다. 인덱스 번호 확인 : {statIndex}");
        }

        public float SubscribeStat(int statIndex, StatSO.ValueChangeHandler handler, float defaultValue)
        {
            if (_statDict.TryGetValue(statIndex, out StatSO stat))
            {
                stat.OnValueChanged += handler;
                return stat.Value;
            }

            return defaultValue;
        }
        
        public void UnSubscribeStat(int statIndex, StatSO.ValueChangeHandler handler)
        {
            if (_statDict.TryGetValue(statIndex, out StatSO stat))
            {
                stat.OnValueChanged -= handler;
            }
        }

        public abstract DamageData CalculateDamage(SkillDataSO skillData);
    }
}