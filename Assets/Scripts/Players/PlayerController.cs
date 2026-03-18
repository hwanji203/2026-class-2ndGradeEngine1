using Agents;
using Agents.FSM;
using UnityEngine;

namespace Players
{
    public class PlayerController : Agent
    {
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
        [SerializeField] private StateSO[] playerStates;
        
        private IControlMovement _movement; 
        private StateMachine _stateMachine;
        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _stateMachine = new StateMachine(this, playerStates);
        }

        private void Start()
        {
            _stateMachine.ChangeState(0, transitionDuration: 0); // 가독성을 위해서
        }

        private void Update()
        {
            _stateMachine.UpdateMachine();
        }
        public void ChangeState(int newStateIndex, float transitionDuration) => _stateMachine.ChangeState(newStateIndex, transitionDuration);
    }
}