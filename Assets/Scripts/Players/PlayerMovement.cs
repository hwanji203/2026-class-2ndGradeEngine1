using UnityEngine;

namespace Players
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private Transform parentTrm;
        [SerializeField] private CharacterController controller;
        [SerializeField] private float rotationSpeed = 0.5f;

        private Vector3 _velocity;
        private float _verticalVelocity;
        private Vector3 _movementDirection;

        public bool IsGround => controller.isGrounded;
        public Vector3 Velocity => _velocity;

        public void SetMovementDirection(Vector2 inputDirection)
        {
            _movementDirection = new Vector3(inputDirection.x, 0f, inputDirection.y);
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            ApplyGravity();
            MoveCharacter();
        }

        private void CalculateMovement()
        {
            _velocity = Quaternion.Euler(0, -45f, 0) * _movementDirection;
            _velocity *= moveSpeed * Time.fixedDeltaTime;

            if (_velocity.sqrMagnitude > 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_velocity);
                parentTrm.rotation = Quaternion.Lerp(parentTrm.rotation, targetRotation,
                    Time.fixedDeltaTime * rotationSpeed);
                //float t = 1 - Mathf.Exp(-rotationSpeed * Time.fixedDeltaTime);
                //parentTrm.rotation = Quaternion.Lerp(parentTrm.rotation, targetRotation, t);
            }
        }

        private void ApplyGravity()
        {
            if (IsGround && _verticalVelocity <= 0)
            {
                _verticalVelocity = -0.3f; //아래로 당기는 힘을 준다.
            }
            else
            {
                _verticalVelocity += gravity * Time.fixedDeltaTime;
            }
            _velocity.y = _verticalVelocity; //중력 적용한 힘을 가한다.
        }

        private void MoveCharacter()
        {
            controller.Move(_velocity);
        }
    }
}