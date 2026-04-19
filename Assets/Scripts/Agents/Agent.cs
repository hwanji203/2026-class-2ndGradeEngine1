using GGMLib.ModuleSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Agents
{
    public class Agent : ModuleOwner
    {
        public bool IsDead { get; set; }

        public UnityEvent OnHit;
        public UnityEvent OnDeath;
    }
}