using System;
using GGMLib.ModuleSystem;
using UnityEngine;

namespace Players
{
    public class PlayerMovement : MonoBehaviour, IModule, IControlMovement
    {
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private CharacterController controller;

        private Vector3 _velocity;
        private float _verticalVelocity;
        private Vector3 _movementDirection;
        private ModuleOwner _owner;
        private Vector3 _manulVelocity;

        public bool IsGround => controller.isGrounded;
        public Vector3 Velocity => _velocity;
        public bool CanManualMove { get; set; } = true;


        public void Initialize(ModuleOwner owner)
        {
            _owner = owner;
        }
        
        public void SetMovementDirection(Vector2 inputDirection)
        {
            _movementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            ApplyGravity();
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            controller.Move(_velocity);
        }

        public void RotateTo(Vector3 direction)
        {
            if (direction.sqrMagnitude < Mathf.Epsilon) return;
            direction.y = 0;
            _owner.transform.forward = direction.normalized;
        }

        private void ApplyGravity()
        {
            if (IsGround && _verticalVelocity <= 0)// 땅에 있고 수직 속력이 0보다 작으면
            {
                _verticalVelocity = -0.3f; // 아래로 당기는 힘을 준다
            }
            else
            {
                _verticalVelocity += gravity * Time.fixedDeltaTime; // 중력 적용한 힘을 가한다
            }

            _velocity.y = _verticalVelocity;
        }

        private void CalculateMovement()
        {
            if (CanManualMove)
                _velocity = Quaternion.Euler(0, -45f, 0) * _movementDirection;
            else
                _velocity = _manulVelocity;

            _velocity *= moveSpeed * Time.deltaTime;

            if(_velocity.sqrMagnitude > 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_velocity);
                _owner.transform.rotation = Quaternion.Lerp(_owner.transform.rotation, targetRotation, Time.deltaTime * 10);
            }
        }

        public void SetMovementVelocity(Vector3 velocity)
        {
            _manulVelocity = velocity;
        }
    }
}