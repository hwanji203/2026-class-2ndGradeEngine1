using Assets.GGMLib.AnimationSystem;
using UnityEngine;

namespace Assets.Scripts.Agent.FSM
{
    [CreateAssetMenu(fileName = "State Data", menuName = "Agent/State Data", order = 10)]
    public class StateSO : ScriptableObject
    {
        public string stateName;
        public string className;
        public int assetIndex;
        public AnimParamSO stateParam;
    }
}

