using System;
using Enemies.Nav;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MoveToNextPoint", story: "[Enemy] move to Point", category: "Action/Navigation", id: "8d00ed217a88b03879c42677334435c4")]
    public partial class MoveToNextPointAction : Action
    {
        [SerializeReference] public BlackboardVariable<AbstractEnemy> Enemy;

        private INavMovement _navMovement;
        
        protected override Status OnStart()
        {
            if (Enemy.Value == null || Enemy.Value.NavMovement == null || Enemy.Value.WayPoints == null)
                return Status.Failure;
    
            _navMovement = Enemy.Value.NavMovement;
            WayPointManager wayPointManager = Enemy.Value.WayPoints;
    
            //현 위치에서 가장 가까운 웨이포인트를 찾아서.
            int index = Enemy.Value.CurrentWayPointIndex;
    
            index = index < 0 ?
                wayPointManager.GetClosestPointIndexFromPosition(Enemy.Value.transform.position)
                : wayPointManager.GetNextWayPointIndex(index);
    
            if (index < 0)
                return Status.Failure;
    
            Enemy.Value.CurrentWayPointIndex = index; //갱신
    
            WayPoint targetPoint = wayPointManager[index]; //인덱서로 접근 가능
            _navMovement.SetDestination(targetPoint.Position); //이동명령 
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return _navMovement.IsArrived ? Status.Success : Status.Running;
        }
    }
}

