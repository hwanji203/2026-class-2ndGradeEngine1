using Agents;
using Agents.FSM;
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
            _player.PlayerInput.OnMovementChanged += HandleMovementChange;
        }

        public override void Update()
        {
            base.Update();
        }

        private void HandleMovementChange(Vector2 movementKey)
        {
            _controlMovement.SetMovementDirection(movementKey);
            
            if (movementKey.magnitude < INPUT_DEADZONE)
            {
                _player.ChangeState(0, transitionDuration: 0.1f); // RUN -> IDLE
                return;
            }
        }

        public override void Exit()
        {
            base.Exit();
            _player.PlayerInput.OnMovementChanged -= HandleMovementChange;
        }
    }
}