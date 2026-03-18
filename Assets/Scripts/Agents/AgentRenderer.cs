using GGMLib.ModuleSystem;
using UnityEngine;

namespace Agents
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
            // 이전 애니메이션에서 부드럽게 전환
            Animator.CrossFadeInFixedTime(clipHash, crossFadeDuration, layerIndex, normalizedTime);
        }
    }
}