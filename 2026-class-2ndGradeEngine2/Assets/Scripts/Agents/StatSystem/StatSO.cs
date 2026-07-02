using System;
using System.Collections.Generic;
using GGMLib.DatabaseSystem.Runtime;
using UnityEngine;

namespace Agents.StatSystem
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "Agent/Stat data", order = 15)]
    public class StatSO : IndexedAsset, ICloneable
    {
        public delegate void ValueChangeHandler(StatSO stat, float currentValue, float prevValue);
        public event ValueChangeHandler OnValueChanged;
        
        [field: SerializeField] public string StatName { get; private set; }
        [field: SerializeField] public Sprite StatIcon { get; private set; }
        [field: SerializeField] public string DisplayName { get; private set; }
        [field: SerializeField] public bool IsPercent { get; private set; }

        [SerializeField] private float baseValue;
        [SerializeField] private float minValue, maxValue;
        [SerializeField, TextArea] private string description;

        private Dictionary<object, float> _modifyValueByKey = new Dictionary<object, float>();
        private float _modifiedvalue = 0;

        public float MaxValue => maxValue;
        public float MinValue => minValue;

        public float Value => Mathf.Clamp(baseValue + _modifiedvalue, MinValue, MaxValue);
        public bool IsMax => Mathf.Approximately(baseValue, maxValue);
        public bool IsMin => Mathf.Approximately(baseValue, minValue);

        public float BaseValue
        {
            get => baseValue;
            set
            {
                float prevValue = Value;
                baseValue = Mathf.Clamp(value, MinValue, MaxValue);
                TryInvokeValueChangedEvent(Value, prevValue);
            }
        }

        public void AddModifier(object key, float value)
        {
            if (_modifyValueByKey.ContainsKey(key)) return; //1중첩만 허용

            float prevValue = Value;
            _modifyValueByKey.Add(key, value);
            _modifiedvalue += value;
            
            TryInvokeValueChangedEvent(Value, prevValue);
        }

        public void RemoveModifier(object key)
        {
            if (_modifyValueByKey.TryGetValue(key, out float value))
            {
                float prevValue = Value;
                _modifyValueByKey.Remove(key);
                _modifiedvalue -= value;
                
                TryInvokeValueChangedEvent(Value, prevValue);
            }
        }

        private void TryInvokeValueChangedEvent(float current, float prevValue)
        {
            if (Mathf.Approximately(current, prevValue))
                OnValueChanged?.Invoke(this, current,prevValue);
        }

        public object Clone()
        {
            return Instantiate(this);
        }
    }
}