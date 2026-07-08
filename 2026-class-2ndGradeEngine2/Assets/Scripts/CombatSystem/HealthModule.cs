using System;
using Agents;
using Agents.StatSystem;
using GGMLib.ModuleSystem;
using UnityEngine;

namespace CombatSystem
{
    public class HealthModule : MonoBehaviour, IModule, IAfterInitModule
    {
        public event Action OnDeath;
        [SerializeField] private StatSO vitalStat;
        [SerializeField] private float baseMaxHealth;
        [SerializeField] private float maxHealth; //나중에 스탯으로 변경된다.
        [SerializeField] private float currentHealth; //디버그 용도로 직렬화해두었다.
    
        private ModuleOwner _owner;
        private IStatModule _statModule;
        private float _vitalValue;
        
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            _statModule = _owner.GetModule<IStatModule>();
        }

        public void AfterInit()
        {
            if (_statModule != null)
            {
                _vitalValue = _statModule.SubscribeStat(vitalStat.AssetIndex, HandleVitalChange, 1);
            }

            currentHealth = maxHealth = baseMaxHealth + 5 * _vitalValue;
        }

        private void OnDestroy()
        {
            _statModule?.UnSubscribeStat(vitalStat.AssetIndex, HandleVitalChange);
        }

        private void HandleVitalChange(StatSO stat, float currentValue, float prevValue)
        {
            _vitalValue = currentValue;
            float beforeMaxHealth = maxHealth;
            maxHealth = baseMaxHealth + 5 * _vitalValue;
            float delta = maxHealth - beforeMaxHealth;
            currentHealth = Mathf.Clamp(currentHealth + delta, 1, maxHealth);
        }

        public void ApplyDamage(float damageAmount)
        {
            currentHealth -= damageAmount;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDeath?.Invoke();
            }
        }
    }
}