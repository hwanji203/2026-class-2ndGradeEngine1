using UnityEngine;

namespace Agents
{
    public interface IRenderer
    {
        public Animator Animator { get; }
        void PlayClip(int clipHash, float normalizedTime, float crossFadeDuration, int layerIndex = 0);
    }
}