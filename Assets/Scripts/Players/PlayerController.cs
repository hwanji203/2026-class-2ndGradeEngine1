using UnityEngine;

namespace Players
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private PlayerMovement movement;

        private void Awake()
        {
            playerInput.OnMovementChange += HandleMovementChange;
        }

        private void OnDestroy()
        {
            playerInput.OnMovementChange -= HandleMovementChange;
        }

        private void HandleMovementChange(Vector2 inputDirection)
        {
            movement.SetMovementDirection(inputDirection);
        }
    }
}

