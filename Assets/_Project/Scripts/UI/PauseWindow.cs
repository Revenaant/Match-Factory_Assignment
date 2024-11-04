using System.Collections;
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

        // Handling pause would ideally be in a more centralized system
        // There are no other major systems to consider for pause so far, so keeping it simple
        [SerializeField] private InteractablesManager interactablesManager;
        private bool isPaused;

        public override void Initialize()
        {
            base.Initialize();
            restartButton.ClickedEvent += OnRestartClicked;
            mainMenuButton.ClickedEvent += OnMainMenuClicked;

            this.EvtHidden += OnWindowHidden;
        }

        public override void Uninitialize()
        {
            base.Uninitialize();
            restartButton.ClickedEvent -= OnRestartClicked;
            mainMenuButton.ClickedEvent -= OnMainMenuClicked;

            this.EvtHidden -= OnWindowHidden;
        }

        public override IEnumerator ShowEnumerator()
        {
            SetPaused(true);
            yield return base.ShowEnumerator();
        }

        private void OnWindowHidden(Window window)
        {
            SetPaused(false);
        }

        private void SetPaused(bool pause) 
        {
            Physics.autoSimulation = !pause;
            interactablesManager.SetPaused(pause);
            isPaused = pause;
        }

        private void OnRestartClicked()
        {
            SetPaused(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnMainMenuClicked()
        {
            SetPaused(false);
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
