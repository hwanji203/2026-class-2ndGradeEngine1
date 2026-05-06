using GGMLib.ModuleSystem;
using UnityEngine.Events;

namespace Agents
{
    public abstract class Agent : ModuleOwner
    {
        public bool IsDead { get; set; }

        public UnityEvent OnHit;
        public UnityEvent OnDeath;
    }
}