using Assets.Scripts.Agent;
using GGMLib;
using UnityEngine;

namespace Agnets
{
    [RequireComponent(typeof(Animator))]
    public class AgentRenderer : MonoBehaviour, IModule, IRenderer
    {
        public Animator Animator { get; private set; }
        private ModuleOwner _owner;

        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
            Animator = GetComponent<Animator>();
        }

        public void PlayClip(int clipHash, float normalizedTime, float crossFadeDuration, int layerIndex = 0)
        {
            Animator.CrossFadeInFixedTime(clipHash, crossFadeDuration, layerIndex, normalizedTime);
        }
    }
}

