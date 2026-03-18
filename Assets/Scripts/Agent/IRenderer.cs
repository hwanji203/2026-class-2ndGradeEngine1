using UnityEngine;

namespace Assets.Scripts.Agent
{
    public interface IRenderer
    {
        public void PlayClip(int clipHash, float normalizedTime, float crossFadeDuration, int layerIndex = 0);
    }

}
