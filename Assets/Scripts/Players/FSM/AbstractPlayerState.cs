using Agents;
using Assets.Scripts.Agent.FSM;
using UnityEngine;

namespace Players.FSM
{
    public abstract class AbstractPlayerState : AgentState
    {
        protected PlayerController _player;
        protected IControlMovement _controlMovement;
        protected const float INPUT_DEADZONE = 0.1f;

        protected AbstractPlayerState(Agent agent, int stateClipHash) : base(agent, stateClipHash)
        {
            _player = agent as PlayerController;
            _controlMovement = agent.GetModule<IControlMovement>();
        }
    }

}
