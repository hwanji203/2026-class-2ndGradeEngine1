using Agents;
using Agents.FSM;
using UnityEngine;

namespace Players.FSM
{
    public class PlayerIdleState : AbstractPlayerState
    {
        public PlayerIdleState(Agent agent, int stateClipHash) : base(agent, stateClipHash)
        {
        }

        public override void Enter(float transitionDuration, int layerIndex = 0)
        {
            base.Enter(transitionDuration, layerIndex);
            _controlMovement.SetMovementDirection(Vector2.zero);
            _player.PlayerInput.OnMovementChanged += HandleMovementChange;
        }

        public override void Exit()
        {
            base.Exit();
            _player.PlayerInput.OnMovementChanged -= HandleMovementChange;
        }

        private void HandleMovementChange(Vector2 movementKey)
        {
            if (movementKey.magnitude > INPUT_DEADZONE)
            {
                _player.ChangeState(1, transitionDuration: 0.1f); // IDLE -> RUN
            }
        }
    }
}