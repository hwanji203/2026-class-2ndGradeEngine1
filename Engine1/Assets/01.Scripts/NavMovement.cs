using Players;
using UnityEngine;
using UnityEngine.AI;

namespace _01.Scripts
{
    public class NavMovement : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;

        private NavMeshAgent _navAgent;

        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            playerInput.OnAttackKeyPressed += HandleMouseClick;
        }

        private void OnDestroy()
        {
            playerInput.OnAttackKeyPressed  -= HandleMouseClick;
        }

        private void HandleMouseClick()
        {
            Vector3 position = playerInput.GetWorldMousePosition();
            _navAgent.SetDestination(position);
        }
    }
}