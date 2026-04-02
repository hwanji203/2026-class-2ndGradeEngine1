using UnityEngine;

namespace CoreSystem.Effect
{
    public interface IPlayableVFX
    {
        AssetNameSO VfxName { get; }
        void PlayVFX(Vector3 position, Quaternion rotation);
        void PlayVFX();
        void StopVFX();
    }
}