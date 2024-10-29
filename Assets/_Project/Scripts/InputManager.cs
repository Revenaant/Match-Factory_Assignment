using Revenaant.Project.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Revenaant.Project
{
    public class InputManager : MonoBehaviour, GameplayInput.IGameplayActions
    {
        [SerializeField] private TextMeshProUGUI debugText;

        private Camera mainCamera;
        private GameplayInput gameplayControls;

        private void Awake()
        {
            gameplayControls = new GameplayInput();
            gameplayControls.Gameplay.SetCallbacks(this);

            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            gameplayControls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            gameplayControls.Gameplay.Disable();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                debugText.text = "Performed";
            }

            if (!context.canceled)
                return;

            Vector2 pointerPosition = gameplayControls.Gameplay.TouchPosition.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(pointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                debugText.text = "Hit object + " + hit.transform.name;
                Interactable r = hit.collider.GetComponentInParent<Interactable>();
                if (r != null)
                {
                    debugText.text += " Is Interactable!";
                    r.Interact();
                }
            }
        }

        public void OnTouchPosition(InputAction.CallbackContext context)
        {
        }
    }
}
