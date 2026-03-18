using Assets.Scripts.Agent.FSM;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace Agents.FSM.Editor
{
    [CustomEditor(typeof(StateSO))]
    public class StateSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorView = default;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            editorView.CloneTree(root);

            return root;
        }
    }
}

