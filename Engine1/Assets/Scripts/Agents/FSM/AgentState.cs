using Unity.VisualScripting;

namespace Agents.FSM
{
    public abstract class AgentState
    {
        protected readonly Agent _agent;
        protected readonly int _stateClipClipHash;
        protected readonly IRenderer _renderer;

        public AgentState(Agent agent, int stateClipHash)
        {
            _agent = agent;
            _stateClipClipHash = stateClipHash;
            _renderer = _agent.GetModule<IRenderer>();
        }

        public virtual void Enter(float transitionDuration, int layerIndex = 0)
        {
            _renderer.PlayClip(_stateClipClipHash, 0f, transitionDuration, layerIndex);
        }
        
        public virtual void Update(){}
        public virtual void Exit(){}
    }
}