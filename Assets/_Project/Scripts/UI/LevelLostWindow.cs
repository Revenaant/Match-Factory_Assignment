using Revenaant.Project.Messages;
using TMPro;
using UnityEngine;

namespace Revenaant.Project.UI
{
    public class LevelLostWindow : PauseWindow
    {
        [SerializeField] private TextMeshProUGUI reasonText;

        public void SetupEndWindow(GameOverType gameOverType)
        {   
            reasonText.text = gameOverType.ToString();
        }
    }
}
