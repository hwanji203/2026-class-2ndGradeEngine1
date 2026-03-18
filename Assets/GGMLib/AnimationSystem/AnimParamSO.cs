using System;
using UnityEngine;

namespace GGMLib.AnimationSystem
{
    [CreateAssetMenu(fileName = "Animator param", menuName = "Lib/Animator Param", order = 0)]
    public class AnimParamSo : ScriptableObject
    {
        [field: SerializeField] public string ParamName { get; private set; }
        [field: SerializeField] public int ParamHash { get; private set; }

        private void OnValidate()
        {
            ParamHash = Animator.StringToHash(ParamName);
        }
    }
}