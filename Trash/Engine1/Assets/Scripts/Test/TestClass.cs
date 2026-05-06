using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor
{
    public class TestClass : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset _contentAsset;
        
        private UIDocument _document;
        private VisualElement _root;
        private VisualElement _content;

        private Button _elementBtn1;
        private Button _elementBtn2;
        private Button _elementBtn3;
        
        private void Awake()
        {
            _document = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _root = _document.rootVisualElement;

            _content = _root.Q<VisualElement>("ContentContainer");
            _elementBtn1 = _root.Q<Button>("WhiteButton");
            
            _elementBtn1.RegisterCallback<ClickEvent>(HandleContentChange);
        }

        private void HandleContentChange(ClickEvent evt)
        {
            _content.Clear();
            _contentAsset.CloneTree(_content);
        }
    }
}