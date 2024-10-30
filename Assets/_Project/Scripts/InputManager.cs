using Revenaant.Project.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Revenaant.Project
{
    // Rename to InteractablesManager?
    public class InputManager : MonoBehaviour, GameplayInput.IGameplayActions
    {
        [SerializeField] private TextMeshProUGUI debugText;

        private Camera mainCamera;
        private GameplayInput gameplayControls;

        private Interactable selectedInteractable;

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
            if (context.canceled && selectedInteractable != null)
            {
                selectedInteractable.ToggleHighlight(false);
                selectedInteractable.Interact();
                selectedInteractable = null;
            }
        }

        public void OnTouchPosition(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            Vector2 pointerPosition = context.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(pointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit) 
                && hit.collider.TryGetComponentInParent(out Interactable interactable))
            {
                if (selectedInteractable != interactable)
                {
                    selectedInteractable?.ToggleHighlight(false);
                    selectedInteractable = interactable;
                    selectedInteractable?.ToggleHighlight(true);
                }
            }
            else if (selectedInteractable != null)
            {
                selectedInteractable.ToggleHighlight(false);
                selectedInteractable = null;
            }
        }
    }
}
