using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test
{
    public class TestMainPanelUI : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset[] contentAsset;

        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _popUpWindow;
        private VisualElement _popUpContent;
        private VisualElement _selectBtns;
        private VisualElement _contentVisual;
        private Button[] _buttons;
        private Button _selectedButton;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _root = _uiDocument.rootVisualElement;
            _popUpWindow = _root.Q<VisualElement>("PopUpWindow");
            _popUpContent = _root.Q<VisualElement>("Content");

            VisualElement topContainer = _root.Q<VisualElement>("TopContainer");
            topContainer.RegisterCallback<ClickEvent>(HandleBtnClick);

            Button closeBtn = _root.Q<Button>("CloseBtn");
            closeBtn.RegisterCallback<ClickEvent>(HandleCloseButton);

            _selectBtns = _root.Q<VisualElement>("SelectBtns");
            _contentVisual = _root.Q<VisualElement>("ContentVisual");
            _selectBtns.RegisterCallback<ClickEvent>(SelectButtonHandler);
            _buttons = _selectBtns.Query<Button>().ToList().ToArray();
        }

        private void SelectButtonHandler(ClickEvent evt)
        {
            if (evt.target is Button button)
            {
                switch (button.text)
                {
                    case "장비":
                        ChangeA(0);
                        break;
                    case "인벤토리":
                        ChangeA(1);
                        break;
                    case "세팅":
                        ChangeA(2);
                        break;
                }
            }
        }

        private void HandleCloseButton(ClickEvent evt)
        {
            _popUpWindow.RemoveFromClassList("open");
        }

        private void HandleBtnClick(ClickEvent evt)
        {
            if (evt.target is DataButton dataBtn)
            {
                OpenPopUpWindow();
            }
        }

        private void OpenPopUpWindow()
        {
            ChangeA(0);

            _popUpWindow.AddToClassList("open");

        }

        private void ChangeA(int idx)
        {
            _contentVisual.Clear(); //자식들이 전부 없어져.
            contentAsset[idx].CloneTree(_contentVisual); // 자식으로 복사해서 생성해줘.
            if (_selectedButton != null)
            {
                _selectedButton.RemoveFromClassList("on");
            }
            _selectedButton = _buttons[idx];
            _selectedButton.AddToClassList("on");
        }
    }
}

