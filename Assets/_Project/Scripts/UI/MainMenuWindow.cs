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
            playButton.ClickedEvent += OnPlayClicked;
            optionsButton.ClickedEvent += OnOptionsClicked;
            storeButton.ClickedEvent += OnStoreClicked;

            SetInteractability(true);
            LevelConfigProvider.InitializeProvider();

            // Hacky fixing a stuttering issue on Android
            // https://www.reddit.com/r/Unity3D/comments/sbfppu/psa_for_android_frame_stuttering/
            Application.targetFrameRate = 61;
        }

        public override void Uninitialize()
        {
            base.Uninitialize();
            playButton.ClickedEvent -= OnPlayClicked;
            optionsButton.ClickedEvent -= OnOptionsClicked;
            storeButton.ClickedEvent -= OnStoreClicked;
        }

        private void OnPlayClicked()
        {
            LevelConfigProvider.Instance.RandomizeLevel();
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
