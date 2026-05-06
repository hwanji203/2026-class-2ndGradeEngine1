using System;
using Agents;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FindTarget", story: "[Enemy] Find [TargetGameObject]", category: "Action/Combat", id: "1d40d992aaf4efd5f44ab1ad68756473")]
    public partial class FindTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

        protected override Status OnStart()
        {
            if (Enemy.Value == null || Enemy.Value.Sensor == null || TargetGameObject.Value != null)
                return Status.Failure;
            
            AgentSensor sensor = Enemy.Value.Sensor;

            int detectCount = sensor.FindTargetsInRadius(Enemy.Value.DetectRadius);
            if (detectCount <= 0) return Status.Failure;
            
            Transform targetTrm  = sensor.ColliderResults[0].transform;
            
            if (!sensor.IsTargetInViewAngle(targetTrm, Enemy.Value.ViewAngle))
                return Status.Failure; // 시야각 
            
            if (!sensor.IsTargetIsInSight(targetTrm))
                return Status.Failure;
            
            TargetGameObject.Value = targetTrm.gameObject;
            
            return Status.Running;
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

