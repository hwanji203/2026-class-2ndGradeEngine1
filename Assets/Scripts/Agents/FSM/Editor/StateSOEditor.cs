using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Agents.FSM.Editor
{
    [CustomEditor(typeof(StateSO))]
    public class StateSOEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset editorView = default;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();
            editorView.CloneTree(root);
            
            return root;
        }
    }
}