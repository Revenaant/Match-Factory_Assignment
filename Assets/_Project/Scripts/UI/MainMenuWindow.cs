using Revenaant.Project.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Revenaant.Project
{
    public class MainMenuWindow : Window
    {
        [SerializeField] private SceneReference gameplayScene;

        [SerializeField] private Window optionsWindow;
        [SerializeField] private Window storeWindow;

        [SerializeField] private GenericButton playButton;
        [SerializeField] private GenericButton optionsButton;
        [SerializeField] private GenericButton storeButton;

        public override void Initialize()
        {
            base.Initialize();
            playButton.EvtClicked += OnPlayClicked;
            optionsButton.EvtClicked += OnOptionsClicked;
            storeButton.EvtClicked += OnStoreClicked;

            SetInteractability(true);
        }

        public override void Uninitialize()
        {
            base.Uninitialize();
            playButton.EvtClicked -= OnPlayClicked;
            optionsButton.EvtClicked -= OnOptionsClicked;
            storeButton.EvtClicked -= OnStoreClicked;
        }

        private void OnPlayClicked()
        {
            SceneManager.LoadScene(gameplayScene.ScenePath, LoadSceneMode.Single);
        }

        private void OnOptionsClicked()
        {
            optionsWindow.Show(this);
        }

        private void OnStoreClicked()
        {
            storeWindow.Show(this);
        }
    }
}
