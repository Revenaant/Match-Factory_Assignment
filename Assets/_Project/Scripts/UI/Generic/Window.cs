using System;
using System.Collections;
using UnityEngine;

namespace Revenaant.Project.UI
{
    [SelectionBase]
    public class Window : MonoBehaviour
    {
        public enum WindowState : int
        {
            Hidden = -2,
            Hiding = -1,
            Showing = 1,
            Shown = 2
        }

        private IWindowAnimationController animationController;
        private CanvasGroup canvasGroup;
        private bool isInteractable;
        private WindowState state = WindowState.Hidden;

        public Action<Window> EvtShown;
        public Action<Window> EvtHidden;

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Uninitialize();
        }

        public virtual void Initialize()
        {
            animationController = GetComponent<IWindowAnimationController>();

            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();

            canvasGroup.interactable = false;
        }

        public virtual void Uninitialize()
        {

        }

        public void Show(MonoBehaviour routineOwner)
        {
            Debug.Assert(routineOwner != null);
            routineOwner.StartCoroutine(ShowEnumerator());
        }

        public void Hide(MonoBehaviour routineOwner, bool instantly = false)
        {
            Debug.Assert(routineOwner != null);
            routineOwner.StartCoroutine(HideEnumerator(instantly));
        }

        public virtual IEnumerator ShowEnumerator()
        {
            if (state >= WindowState.Showing)
                yield break;

            state = WindowState.Showing;

            SetInteractability(false);
            ShowInternal();

            IWindowAnimationController animationController = GetAnimationController();
            if (animationController != null)
            {
                yield return animationController.ShowEnumerator();
            }

            state = WindowState.Shown;
            EvtShown?.Invoke(this);
            SetInteractability(true);
        }

        public virtual IEnumerator HideEnumerator(bool instantly = false)
        {
            if (state <= WindowState.Hiding)
                yield break;

            state = WindowState.Hiding;
            SetInteractability(false);

            if (!instantly)
            {
                IWindowAnimationController animationController = GetAnimationController();
                if (animationController != null)
                {
                    yield return animationController.HideEnumerator();
                    animationController.HideInstantly();
                }
            }

            gameObject.SetActive(false);
            state = WindowState.Hidden;
            EvtHidden?.Invoke(this);
        }

        public void SetInteractability(bool value)
        {
            if (isInteractable != value)
            {
                canvasGroup.interactable = value;
                isInteractable = value;
            }
        }

        protected virtual void ShowInternal()
        {
            base.gameObject.SetActive(true);
            Transform parent = base.transform.parent;
            while (parent != null)
            {
                parent.gameObject.SetActive(true);
                parent = parent.parent;
            }
        }

        private IWindowAnimationController GetAnimationController()
        {
            if (animationController == null)
                return null;

            return animationController.CanShow ? animationController : null;
        }
    }
}
