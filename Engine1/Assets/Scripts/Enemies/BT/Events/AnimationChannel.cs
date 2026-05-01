using System;
using GGMLib.AnimationSystem;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

#if UNITY_EDITOR
namespace Enemies.BT.Events
{
    [CreateAssetMenu(menuName = "Behavior/Event Channels/Animation")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "AnimationChannel", message: "Play [Clip]", category: "Events", id: "4e630dce2ca611f62cda1068baaad936")]
    public sealed partial class AnimationChannel : EventChannel<AnimParamSO> { }
}

