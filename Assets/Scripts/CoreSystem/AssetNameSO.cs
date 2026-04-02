using System;
using UnityEngine;

namespace CoreSystem
{
    [CreateAssetMenu(fileName = "AssetName", menuName = "AssetName Data", order  = 0)]
    public class AssetNameSO : ScriptableObject
    {
        [field: SerializeField] public string AssetName { get; private set; }
        public int AssetHash { get; private set; }

        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(this.AssetName))
            {
                AssetHash = Animator.StringToHash(AssetName);
            }
        }
    }
}