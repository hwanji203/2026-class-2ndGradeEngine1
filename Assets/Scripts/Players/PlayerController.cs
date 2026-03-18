using Agents;
using Assets.Scripts.Agent.FSM;
using GGMLib;
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

            _movement = GetModule<IControlMovement>();
            Debug.Assert(_movement != null, "플레이어 이동 관련 모듈이 없음");
        }

        private void Start()
        {
            _stateMachine.ChangeState(0, transitionDuration: 0);
        }
        private void Update()
        {
            _stateMachine.UpdateMachine();
        }

        public void ChangeState(int newStateIndex, float transitionDuration)
            => _stateMachine.ChangeState(newStateIndex, transitionDuration);
    }
}
