using System;
using UnityEngine;
using UnityEngine.UI;

namespace Revenaant.Project.UI
{
    [RequireComponent(typeof(Button))]
    public class GenericButton : MonoBehaviour
    {
        [SerializeField] private Button button;

        public event Action EvtClicked;

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            EvtClicked?.Invoke();
        }

        public void DispatchClickEvent()
        {
            OnButtonClicked();
        }

        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }
    }
}
