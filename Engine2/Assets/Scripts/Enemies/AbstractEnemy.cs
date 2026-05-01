using System;
using Agents;
using Enemies.Nav;
using Reflex.Attributes;
using Unity.Behavior;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(BehaviorGraphAgent))]
    public abstract class AbstractEnemy : Agent
    {
        public INavMovement NavMovement { get; private set; }
        public BehaviorGraphAgent BtAgent { get; private set; }
        public IRenderer AgentRenderer { get; private set; }
        
        [Inject] [field: SerializeField] public WayPointManager WayPoints { get; private set; }
        public int CurrentWayPointIndex { get; set; } = -1; //임시코드

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            NavMovement = GetModule<INavMovement>();
            AgentRenderer = GetModule<IRenderer>();
            BtAgent = GetComponent<BehaviorGraphAgent>();
        }

        protected virtual void Start()
        {
            BtAgent.SetVariableValue<AbstractEnemy>("Enemy", this);
        }
    }
}