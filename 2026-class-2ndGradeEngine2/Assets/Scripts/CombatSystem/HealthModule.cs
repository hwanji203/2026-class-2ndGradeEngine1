using System;
using GGMLib.ModuleSystem;
using UnityEngine;

namespace CombatSystem
{
    public class HealthModule : MonoBehaviour, IModule
    {
        public event Action OnDeath;
        
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
    
        private ModuleOwner _owner;
    
        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            currentHealth = maxHealth;
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