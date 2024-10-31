using UnityEngine;
using UnityEngine.SceneManagement;

namespace Revenaant.Project.UI
{
    public class PauseWindow : GenericPopup
    {
        [SerializeField] private SceneReference mainMenuScene;

        [Header("Buttons")]
        [SerializeField] private GenericButton restartButton;
        [SerializeField] private GenericButton mainMenuButton;

        public override void Initialize()
        {
            base.Initialize();
            restartButton.EvtClicked += OnRestartClicked;
            mainMenuButton.EvtClicked += OnMainMenuClicked;
        }

        public override void Uninitialize()
        {
            base.Uninitialize();
            restartButton.EvtClicked -= OnRestartClicked;
            mainMenuButton.EvtClicked -= OnMainMenuClicked;
        }

        private void OnRestartClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnMainMenuClicked()
        {
            SceneManager.LoadScene(mainMenuScene);
        }

    }
}
