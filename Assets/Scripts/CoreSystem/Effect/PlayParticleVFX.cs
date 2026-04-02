using Unity.VisualScripting;
using UnityEngine;

namespace CoreSystem.Effect
{
    public class PlayParticleVFX : MonoBehaviour, IPlayableVFX
    {
        [field: SerializeField] public AssetNameSO VfxName { get; private set; }
        [SerializeField] private ParticleSystem[] particles;
        
        public void PlayVFX(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            PlayVFX();
        }

        public void PlayVFX()
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }
        }

        public void StopVFX()
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }
        }
    }
}