using Players;
using UnityEngine;

namespace Agents.FSM
{
    public class AbstractPlayerState : AgentState
    {
        protected PlayerController _player;
        protected IControlMovement _controlMovement;
        protected const float INPUT_DEADZONE = 0.1f;
        
        public AbstractPlayerState(Agent agent, int stateClipHash) : base(agent, stateClipHash)
        {
            _player = agent as PlayerController;
            _controlMovement = _player.GetModule<IControlMovement>();
        }
    }
}