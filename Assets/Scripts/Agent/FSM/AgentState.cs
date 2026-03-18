using Agents;
using Agnets;
using UnityEngine;

namespace Assets.Scripts.Agent.FSM
{
    public abstract class AgentState
    {
        protected Agents.Agent _agent;
        protected readonly int _stateClipHash;
        protected readonly IRenderer _renderer;

        public AgentState(Agents.Agent agetn, int stateClipHash)
        {
            _agent = agetn;
            _stateClipHash = stateClipHash;
            _renderer = agetn.GetModule<IRenderer>();
        }

        public virtual void Enter(float transitionDuration, int layerIndex = 0)
        {
            _renderer.PlayClip(_stateClipHash, 0f, transitionDuration, layerIndex);
        }

        public virtual void Update() { }
        public virtual void Exit() { }
    }
}

