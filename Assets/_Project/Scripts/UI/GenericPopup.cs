using UnityEngine;

namespace Revenaant.Project.UI
{
    public class GenericPopup : Window
    {
        [SerializeField] private GenericButton backButton;

        public override void Initialize()
        {
            base.Initialize();
            backButton.EvtClicked += OnBackButtonClicked;
        }

        public override void Uninitialize()
        {
            base.Uninitialize();
            backButton.EvtClicked -= OnBackButtonClicked;
        }

        private void OnBackButtonClicked()
        {
            Hide(this);
        }
    }

}
