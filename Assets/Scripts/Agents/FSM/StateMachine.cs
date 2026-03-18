using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agents.FSM
{
    public class StateMachine
    {
        public AgentState CurrentState { get; private set; }
        private Dictionary<int, AgentState> _stateDict;

        public StateMachine(Agent agent, StateSO[] stateList)
        {
            _stateDict = new();
            foreach (var stateSo in stateList)
            {
                Type type = Type.GetType(stateSo.className); // 클래스 이름으로 타입을 찾음
                Debug.Assert(type != null, $"State class {stateSo.className} not found.");
                // 런타임 중에 생성자를 호출하여 Object를 만들고 AgentState로 강제 캐스팅함
                AgentState agentState = (AgentState)Activator.CreateInstance(type, agent, stateSo.stateParam.ParamHash);
                _stateDict.Add(stateSo.assetIndex, agentState);
            }
        }

        public void ChangeState(int newStateIndex, float transitionDuration = 0.1f)
        {
            CurrentState?.Exit();
            AgentState newState = _stateDict.GetValueOrDefault(newStateIndex);
            Debug.Assert(newState != null, $"찾고자하는 인덱스의 상태가 없음 : {newStateIndex}");

            CurrentState = newState;
            CurrentState.Enter(transitionDuration);
        }

        public void UpdateMachine() => CurrentState?.Update();
    }
}