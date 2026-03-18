using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Agent.FSM
{
    public class StateMachine
    {
        public AgentState CurrentState { get; private set; }
        private Dictionary<int, AgentState> _stateDic;

        public StateMachine(Agents.Agent agent, StateSO[] _stateList)
        {
            _stateDic = new Dictionary<int, AgentState>();
            foreach (StateSO stateData in _stateList)
            {
                Type type = Type.GetType(stateData.className);
                Debug.Assert(type != null, $"타입을 찾는데 실패했습니다. : { stateData.className}");

                AgentState agentState = (AgentState)Activator.CreateInstance(type, agent, stateData.stateParam.ParamHash);
                _stateDic.Add(stateData.assetIndex, agentState);
            }
        }

        public void ChangeState(int newStateIndex, float transitionDuration = 0.1f)
        {
            CurrentState?.Exit();
            AgentState newState = _stateDic.GetValueOrDefault(newStateIndex);
            Debug.Assert(newState != null, $"찾고자하는 인덱스의 상태가 없습니다. : {newStateIndex}");

            CurrentState = newState;
            CurrentState.Enter(transitionDuration);
        }

        public void UpdateMachine() => CurrentState?.Update();
    }
}

