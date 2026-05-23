using System;
using CoreSystem.Effects;
using CoreSystem.Events;
using GGMLib.EventChannelSystem;
using GGMLib.ObjectPool.Runtime;
using UnityEngine;

namespace CoreSystem.Managers
{
    public class CreateManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO createChannel;
        [SerializeField] private PoolManagerSO poolManagerAsset;

        private void Awake()
        {
            createChannel.AddListener<ShowPoolingVfx>(HandleShowPoolingVfx);
        }

        private void OnDestroy()
        {
            createChannel.RemoveListener<ShowPoolingVfx>(HandleShowPoolingVfx);
        }

        private void HandleShowPoolingVfx(ShowPoolingVfx evt)
        {
            PoolableVfx vfx = poolManagerAsset.Pop<PoolableVfx>(evt.ItemData);
            vfx.OnVfxEnd += HandleVfxEnd;
            vfx.PlayVfx(evt.Position, evt.Rotation);
        }

        private void HandleVfxEnd(PoolableVfx vfx)
        {
            vfx.OnVfxEnd -= HandleVfxEnd;
            poolManagerAsset.Push(vfx);
        }
    }
}