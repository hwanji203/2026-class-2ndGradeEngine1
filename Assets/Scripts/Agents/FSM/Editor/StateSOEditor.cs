using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Agents.FSM.Editor
{
    [CustomEditor(typeof(StateSO))]
    public class StateSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorView = default;

        private StateSO _targetData;

        public override VisualElement CreateInspectorGUI()
        {
            _targetData = (StateSO)target; 

            VisualElement root = new();
            editorView.CloneTree(root);

            FillDropdownField(root);
            return root;
        }

        private void FillDropdownField(VisualElement root)
        {
            DropdownField field = root.Q<DropdownField>("ClassNameDropdown");

            Assembly stateAssemly = Assembly.GetAssembly(typeof(StateSO));
            IEnumerable<string> choices = stateAssemly.GetTypes()
                .Where(type => type.IsClass 
                    && !type.IsAbstract
                    && type.IsSubclassOf(typeof(AgentState)))
                .Select(type => type.FullName);

            field.choices.AddRange(choices);

            if (_targetData != null && !string.IsNullOrEmpty(_targetData.className)
                && field.choices.Contains(_targetData.className))
            {
                field.value = _targetData.className;
            } else if (_targetData != null && field.choices.Count > 0)
                {
                _targetData.className = field.choices.First();
                EditorUtility.SetDirty(_targetData);
            }

            AssetDatabase.SaveAssetIfDirty(_targetData);
        }
    }
}