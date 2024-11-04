using System.Collections;
using UnityEngine.Playables;
using UnityEngine;

namespace Revenaant.Project.UI
{
    public class WindowTimelineController : MonoBehaviour, IWindowAnimationController
    {
        [SerializeField] private PlayableDirector showTimeline;
        [SerializeField] private PlayableDirector hideTimeline;

        private WaitForSeconds waitForShow;
        private WaitForSeconds waitForHide;

        public bool CanShow => true;

        private void Awake()
        {
            if (showTimeline != null)
                waitForShow = new WaitForSeconds((float)showTimeline.duration);

            if (hideTimeline != null)
                waitForHide = new WaitForSeconds((float)hideTimeline.duration);
        }

        public IEnumerator ShowEnumerator()
        {
            if (showTimeline != null)
            {
                showTimeline.Play();
                yield return waitForShow;
            }
        }

        public IEnumerator HideEnumerator()
        {
            if (hideTimeline != null)
            {
                hideTimeline.Play();
                yield return waitForHide;
            }
        }

        public void HideInstantly()
        {
            if (hideTimeline != null)
            {
                hideTimeline.time = hideTimeline.duration;
                hideTimeline.Evaluate();
            }
        }

        public void SetShowTimeline(PlayableDirector playable)
        {
            showTimeline = playable;
        }
    }
}
