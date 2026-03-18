using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test
{
    public class TestMainPanelUI : MonoBehaviour
    {
        [SerializeField] private Test[] tests;

        private Dictionary<Button, VisualTreeAsset> _test = new();
        
        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _popUpwindow;
        private VisualElement _popUpContent;

        private VisualElement _contentImage;

        private Button _contentBtn1;
        private Button _contentBtn2;
        private Button _contentBtn3;

        private Button _previousBtn;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();


        }

        private void OnEnable()
        {
            _root = _uiDocument.rootVisualElement;
            _popUpwindow = _root.Q<VisualElement>("PopUpWindow");
            
            _popUpContent = _root.Q<VisualElement>("Content");
            
            VisualElement topContainer = _root.Q<VisualElement>("TopContainer");
            topContainer.RegisterCallback<ClickEvent>(HandleButtonClick);

            _contentImage = _root.Q<VisualElement>("ContentImage");
            
            _contentBtn1 = _root.Q<Button>("SelectButton1");
            _contentBtn2 = _root.Q<Button>("SelectButton2");
            _contentBtn3 = _root.Q<Button>("SelectButton3");
            
            _contentBtn1.RegisterCallback<ClickEvent>(ContentClickHandler);
            _contentBtn2.RegisterCallback<ClickEvent>(ContentClickHandler1);
            _contentBtn3.RegisterCallback<ClickEvent>(ContentClickHandle2);
            
            Button closeBtn = _popUpwindow.Q<Button>("CloseBtn");
            closeBtn.RegisterCallback<ClickEvent>(ClosePopUpWindow);
        }

        private void ContentClickHandler(ClickEvent evt)
        {
            _previousBtn?.RemoveFromClassList("clicked");
            _contentBtn1.AddToClassList("clicked");

            _previousBtn = _contentBtn1;
            _contentImage.Clear();
            var visual = tests.FirstOrDefault(x => x.index == 1).asset;
            visual.CloneTree(_contentImage);
        }
        private void ContentClickHandler1(ClickEvent evt)
        {
            _previousBtn?.RemoveFromClassList("clicked");
            _contentBtn2.AddToClassList("clicked");

            _previousBtn = _contentBtn2;
            _contentImage.Clear();
            var visual = tests.FirstOrDefault(x => x.index == 2).asset;
            visual.CloneTree(_contentImage);
        }
        private void ContentClickHandle2(ClickEvent evt)
        {
            _previousBtn?.RemoveFromClassList("clicked");
            _contentBtn3.AddToClassList("clicked");
            
            _previousBtn = _contentBtn3;
            _contentImage.Clear();
            var visual = tests.FirstOrDefault(x => x.index == 3).asset;
            visual.CloneTree(_contentImage);
        }
        private void ClosePopUpWindow(ClickEvent evt)
        {
            _popUpwindow.RemoveFromClassList("open");
        }

        private void HandleButtonClick(ClickEvent evt)
        {
            if (evt.target is DataButton { ButtonIndex: 1 } dataBtn)
            {
                OpenPopUpWindow();
            }
        }

        private void OpenPopUpWindow()
        {
            
            _popUpwindow.AddToClassList("open");
        }

    }

    [Serializable]
    public struct Test
    {
        public int index;
        public VisualTreeAsset asset;
    }
}
