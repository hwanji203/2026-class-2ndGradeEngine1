using GGMLib.ModuleSystem;
using UnityEngine;

namespace CombatSystem
{
    public abstract class AbstractDamageCaster : MonoBehaviour
    {
        [SerializeField] private LayerMask whatIsEnemy;
        
        public ModuleOwner CasterOwner { get; private set; }

        public virtual void IntiCaster(ModuleOwner owner)
        {
            CasterOwner = owner;
        }

        public abstract void CastDamage(Vector3 position, Vector3 direction, SkillDataSO skillData);
    }
}