using Revenaant.Project.Input;
using Revenaant.Project.Messages;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Revenaant.Project
{
    public class InteractablesManager : MonoBehaviour, GameplayInput.IGameplayActions
    {
        private Camera mainCamera;
        private GameplayInput gameplayControls;

        private Interactable selectedInteractable;

        private bool isPaused;

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
            if (!context.canceled || isPaused)
                return;

            if (selectedInteractable != null)
            {
                selectedInteractable.Highlighter.ToggleHighlight(false);
                selectedInteractable.Interact();
                selectedInteractable = null;
            }
        }

        public void OnTouchPosition(InputAction.CallbackContext context)
        {
            if (!context.performed || isPaused)
                return;

            Vector2 pointerPosition = context.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(pointerPosition);

            if (Physics.Raycast(ray, out RaycastHit hit) 
                && hit.collider.TryGetComponentInParent(out Interactable interactable))
            {
                if (selectedInteractable != interactable)
                {
                    selectedInteractable?.Highlighter.ToggleHighlight(false);
                    selectedInteractable = interactable;
                    selectedInteractable?.Highlighter.ToggleHighlight(true);
                }
            }
            else if (selectedInteractable != null)
            {
                selectedInteractable.Highlighter.ToggleHighlight(false);
                selectedInteractable = null;
            }
        }

        public void SetPaused(bool paused)
        {
            isPaused = paused;
        }
    }
}
