using UnityEngine;

namespace Agents.FSM
{
    [CreateAssetMenu(fileName = "State list data", menuName = "Agent/State list")]
    public class StateListSO : ScriptableObject
    {
        [HideInInspector] public string generatePath;
        public string enumName;
        public StateSO[] states;
    }
}

