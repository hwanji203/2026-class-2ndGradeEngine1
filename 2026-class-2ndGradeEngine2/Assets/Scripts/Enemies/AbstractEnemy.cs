using Agents;
using CombatSystem;
using Enemies.Nav;
using Reflex.Attributes;
using Unity.Behavior;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(BehaviorGraphAgent))]
    public abstract class AbstractEnemy : Agent
    {
        [field: SerializeField] public float DetectRadius { get; set; } = 5f; //감지 거리
        [field: SerializeField] public float ViewAngle { get; set; } = 160f; //시야각
        [field: SerializeField] public float StopDistance { get; set; } = 1f; //정지 최소 거리
        
        public INavMovement NavMovement { get; private set; }
        public BehaviorGraphAgent BtAgent { get; private set; }
        public IRenderer AgentRenderer { get; private set; }
        public AgentSensor Sensor { get; private set; }
        public ISkillModule SkillModule { get; private set; }
        
        [Inject] [field: SerializeField] public WayPointManager WayPoints { get; private set; }
        public int CurrentWayPointIndex { get; set; } = -1; //임시코드
        
        [SerializeField] private bool canDrawDebug = false;
        
        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            NavMovement = GetModule<INavMovement>();
            AgentRenderer = GetModule<IRenderer>();
            BtAgent = GetComponent<BehaviorGraphAgent>();
            Sensor = GetModule<AgentSensor>();
            SkillModule = GetModule<ISkillModule>();
        }

        protected virtual void Start()
        {
            BtAgent.SetVariableValue<AbstractEnemy>("Enemy", this);
        }

        private void OnDrawGizmos()
        {
            if (!canDrawDebug) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, DetectRadius);
        }
    }
}