using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test
{
    public class TestMainPanelUI : MonoBehaviour
    {
        [SerializeField] private VisualTreeAsset contentAsset;

        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _popUpWindow;
        private VisualElement _popUpContent;

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
            _popUpContent.Clear(); //자식들이 전부 없어져.
            contentAsset.CloneTree(_popUpContent); // 자식으로 복사해서 생성해줘.

            _popUpWindow.AddToClassList("open");

        }
    }
}

