using System;
using System.Collections;
using GGMLib.ObjectPool.Runtime;
using UnityEngine;

namespace CoreSystem.Effects
{
    public class PoolableVfx : AbstractMonoPoolable
    {
        [SerializeField] private GameObject effectObject;
        private IPlayableVFX _playableVfx;

        public event Action<PoolableVfx> OnVfxEnd;

        private void Awake()
        {
            _playableVfx = effectObject.GetComponent<IPlayableVFX>();
        }

        private void OnValidate()
        {
            if(effectObject == null) return;
            _playableVfx = effectObject.GetComponent<IPlayableVFX>();
            if (_playableVfx == null)
                effectObject = null;
        }

        public override void ResetItem()
        {
            _playableVfx.StopVFX();
        }

        //다음시간에 여기 수정.
        public void PlayVfx(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            StartCoroutine(PlayVfxCoroutine());
        }

        private IEnumerator PlayVfxCoroutine()
        {
            _playableVfx.PlayVFX();
            yield return new WaitForSeconds(_playableVfx.VfxDuration);
            OnVfxEnd?.Invoke(this);
        }
        public void PlayVfx() => _playableVfx.PlayVFX();
    }
}