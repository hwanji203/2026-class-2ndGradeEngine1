using System;
using CombatSystem;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ProcessHit", story: "[Enemy] process hit from [TargetGameObject]", category: "Action/Combat", id: "6f43ca9dbb0d3cb93810c77be4d7b30d")]
    public partial class ProcessHitAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

        protected override Status OnStart()
        {
            if (Enemy.Value == null || Enemy.Value.ActionData == null)
                return Status.Failure;
            
            ActionDataModule actionData = Enemy.Value.ActionData;
            TargetGameObject.Value = actionData.Attacker.gameObject;

            RotateToTarget();
            return Status.Success;
        }

        private void RotateToTarget()
        {
            Vector3 direction = (TargetGameObject.Value.transform.position - Enemy.Value.transform.position);
            direction.y = 0;
            Enemy.Value.transform.rotation = Quaternion.LookRotation(direction.normalized);
        }

        protected override Status OnUpdate()
        {
            return Status.Success;
        }

        protected override void OnEnd()
        {
        }
    }
}

