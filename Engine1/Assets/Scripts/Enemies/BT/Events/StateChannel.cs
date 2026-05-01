using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

namespace Enemies.BT.Events
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/StateChannel")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "StateChannel", message: "Change [State]", category: "Events", id: "ecb27c395e172a941c0ec6eff4f2af1c")]
    public sealed partial class StateChannel : EventChannel<EnemyState> { }
}