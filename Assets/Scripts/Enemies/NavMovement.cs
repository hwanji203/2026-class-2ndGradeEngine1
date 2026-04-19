using GGMLib.ModuleSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    public class NavMovement : MonoBehaviour, IModule, INavMovement
    {
        public NavMeshAgent NavMeshAgent { get; private set; }

        public Vector3 Velocity
        {
            get => NavMeshAgent != null ? NavMeshAgent.velocity : Vector3.zero;
            set
            {
                if (NavMeshAgent != null)
                    NavMeshAgent.velocity = value;
            }
        }

        public float Speed
        {
            get => NavMeshAgent != null ? NavMeshAgent.speed : 0f;
            set
            {
                if (NavMeshAgent != null)
                    NavMeshAgent.speed = value;
            }
        }

        public bool IsStopped
        {
            get => NavMeshAgent != null && NavMeshAgent.isStopped;
            set
            {
                if (NavMeshAgent != null)
                    NavMeshAgent.isStopped = value;
            }
        }

        public bool IsArrived => NavMeshAgent != null
        && (!NavMeshAgent.pathPending && NavMeshAgent.remainingDistance < NavMeshAgent.stoppingDistance);

        public void SetDestination(Vector3 destination)
        {
            NavMeshAgent.SetDestination(destination);
        }

        public void Initialize(ModuleOwner owner)
        {
            NavMeshAgent = owner.GetComponent<NavMeshAgent>();
            Debug.Assert(NavMeshAgent != null, "NavMeshAgent Owner should have a NavMeshAgent");
        }

    }
}