using UnityEngine;

namespace Revenaant.Project.UI
{
    public class GenericPopup : Window
    {
        [SerializeField] private GenericButton backButton;

        public override void Initialize()
        {
            base.Initialize();
            backButton.ClickedEvent += OnBackButtonClicked;
        }

        public override void Uninitialize()
        {
            base.Uninitialize();
            backButton.ClickedEvent -= OnBackButtonClicked;
        }

        private void OnBackButtonClicked()
        {
            Hide(this);
        }
    }

}
