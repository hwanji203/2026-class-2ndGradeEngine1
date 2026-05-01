using GGMLib.ModuleSystem;
using System;
using UnityEngine;

namespace Agents
{
    public class AgentTrigger : MonoBehaviour, IModule
    {
        public event Action OnAnimationEnd;
        public void Initialize(ModuleOwner owner)
        {

        }

        private void AnimationEndTrigger() => OnAnimationEnd?.Invoke();
    }
}
