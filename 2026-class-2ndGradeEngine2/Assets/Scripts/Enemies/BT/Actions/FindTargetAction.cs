using System;
using Agents;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FindTarget", story: "[Enemy] find [TargetGameObject]", category: "Action/Combat", id: "57425be3cb7577de2db1a069ed3d1780")]
    public partial class FindTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;
        [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

        protected override Status OnStart()
        {
            if (Enemy.Value == null || Enemy.Value.Sensor == null || TargetGameObject.Value != null)
                return Status.Failure; //이건 나중에 고친다. 

            AgentSensor sensor = Enemy.Value.Sensor;

            int detectCount = sensor.FindTargetsInRadius(Enemy.Value.DetectRadius);
            if (detectCount <= 0) return Status.Failure;
            
            Transform targetTrm = sensor.ColliderResults[0].transform;
            
            if(!sensor.IsTargetInViewAngle(targetTrm, Enemy.Value.ViewAngle))
                return Status.Failure; //시야각 안에 없다면 실패

            if (!sensor.IsTargetIsInSight(targetTrm))
                return Status.Failure; //사이에 장애물이 있다면 감지 안함
            
            TargetGameObject.Value = targetTrm.gameObject;
            
            return Status.Success;
        }
    }
}

