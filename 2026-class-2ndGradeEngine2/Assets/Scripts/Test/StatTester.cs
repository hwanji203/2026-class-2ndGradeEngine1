using Agents;
using Agents.StatSystem;
using UnityEngine;

namespace Test
{
    public class StatTester : MonoBehaviour
    {
        [SerializeField] private AbstractAgentStatModule statModule;
        [SerializeField] private StatSO targetStat;
        [SerializeField] private float modifyValue;

        private bool _isApplied;

        [ContextMenu("Apply Stat Modifiers")]
        private void ApplyStatModifiers()
        {
            if (_isApplied) return;
            _isApplied = true;
            statModule.AddModifier(targetStat.AssetIndex, this, modifyValue);
        }

        [ContextMenu("Remove Stat Modifiers")]
        private void RemoveStatModifiers()
        {
            if(!_isApplied) return;
            _isApplied = false;
            statModule.RemoveModifier(targetStat.AssetIndex, this);
        }
    } 
}