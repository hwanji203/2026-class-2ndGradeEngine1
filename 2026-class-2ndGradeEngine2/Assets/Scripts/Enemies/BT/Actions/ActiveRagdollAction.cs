using System;
using Agents;
using CombatSystem;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "active ragdoll", story: "Active Ragdoll on [Enemy] with [Force]", category: "Action/Combat", id: "0c7a9b58cf6b3adc6e35014d4cac58d8")]
    public partial class ActiveRagdollAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<float> Force = new(500f);

        protected override Status OnStart()
        {
            if (Enemy.Value == null) return Status.Failure;

            RagdollController ragdoll = Enemy.Value.GetModule<RagdollController>();
            
            if (ragdoll == null)
            {
                Debug.LogWarning($"[ActiveRagdollAction]: {Enemy.Value.name}에 RagdollController가 없습니다.");
                return Status.Failure;
            }

            ActionDataModule actionData = Enemy.Value.ActionData;
            Vector3 hitPoint = actionData != null ? actionData.HitPoint : Enemy.Value.transform.position;
            Vector3 hitNormal = actionData != null ? actionData.HitNormal : Vector3.forward;
            
            ragdoll.EnableRagdoll(hitPoint, hitNormal, Force.Value);
            
            return Status.Success;
        }
    }
}

