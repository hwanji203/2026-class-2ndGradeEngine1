using Agents;
using GGMLib;
using UnityEngine;

namespace Players
{
    public class PlayerController : Agent
    {
        [SerializeField] private PlayerInputSO playerInput;
        private IControlMovement _movement;

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _movement = GetModule<IControlMovement>();
            Debug.Assert(_movement != null, "플레이어 이동 관련 모듈이 없음");
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();
            playerInput.OnMovementChange += HandleMovementChange;
        }

        private void OnDestroy()
        {
            if (playerInput != null)
                playerInput.OnMovementChange -= HandleMovementChange;
        }

        private void HandleMovementChange(Vector2 inputDirection)
        {
            _movement.SetMovementDirection(inputDirection);
        }
    }
}
