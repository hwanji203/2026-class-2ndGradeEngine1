using Agents;
using UnityEngine;

namespace Players.FSM
{
    public class PlayerRunState : AbstractPlayerState
    {
        public PlayerRunState(Agent agent, int stateClipHash) : base(agent, stateClipHash)
        {
        }

        public override void Enter(float transitionDuration, int layerIndex = 0)
        {
            base.Enter(transitionDuration, layerIndex);
            _player.PlayerInput.OnMovementChange += HandleMovementChange;
        }

        public override void Exit()
        {
            _player.PlayerInput.OnMovementChange -= HandleMovementChange;
            base.Exit();
        }

        private void HandleMovementChange(Vector2 movementKey)
        {
            if (movementKey.magnitude > INPUT_DEADZONE)
            {
                _player.ChangeState(1, 0.1f); //RUN상태로 전환
            }
        }
    }
}