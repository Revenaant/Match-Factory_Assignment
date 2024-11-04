using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Revenaant.Project.UI
{
    public class LevelWonWindow : PauseWindow
    {
        [SerializeField] private WindowTimelineController timelineController;
        [SerializeField] private PlayableDirector[] starTimelines;

        [SerializeField] private TextMeshProUGUI fullClearText;

        public void SetupEndWindow(int stars, bool isFullClear)
        {
            Debug.Assert(stars >= 0 && stars < 4, "End window stars out of range");

            timelineController.SetShowTimeline(starTimelines[stars - 1]);
            fullClearText.gameObject.SetActive(isFullClear);

            LevelConfigProvider.Instance.IncreaseLevel();
        }
    }
}
