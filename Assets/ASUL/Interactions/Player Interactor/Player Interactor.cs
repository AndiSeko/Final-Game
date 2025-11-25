using ASUL.InputSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ASUL.Interactions
{
    public class PlayerInteractor : MonoBehaviour
    {
        [field: SerializeField] public bool IsAvailable { get; set; } = true;

        [SerializeField] private float interactionDistance = 1f;
        [SerializeField] private LayerMask interactableMask;
        [SerializeField] private PlayerInteractorViewier viewier;

        public InteractableObject currentTarget;

        private InputAction interactionEvent;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;

            if (InputManager.Instance != null)
            {
                interactionEvent = InputManager.Instance.actions.Player.Interact;
            }
        }

        private void OnEnable()
        {
            if (interactionEvent != null)
            {
                interactionEvent.Enable();
                interactionEvent.performed += OnInteractPerformed;
            }
        }

        private void OnDisable()
        {
            if (interactionEvent != null)
            {
                interactionEvent.performed -= OnInteractPerformed;
                interactionEvent.Disable();
            }
        }

        private void Update()
        {
            Detection();
        }

        private void OnInteractPerformed(InputAction.CallbackContext ctx)
        {
            Interact();
        }

        private void Interact()
        {
            if (currentTarget != null && currentTarget.IsInteractable)
            {
                currentTarget.Interact();
            }
        }

        private void Detection()
        {
            if (!IsAvailable)
            {
                ClearTarget();
                return;
            }

            Vector3 screenCenter = new(Screen.width / 2f, Screen.height / 2f, 0f);
            Ray ray = mainCamera.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableMask))
            {
                var interactable = hit.collider.GetComponent<InteractableObject>();

                if (interactable != null && interactable.IsInteractable)
                {
                    if (currentTarget != interactable)
                    {
                        ClearTarget();
                        currentTarget = interactable;
                        currentTarget.SelectionToggle(true);
                        viewier?.ToggleCursorSelection(true);
                    }
                    return;
                }
            }

            ClearTarget();
        }

        private void ClearTarget()
        {
            if (currentTarget != null)
            {
                currentTarget.SelectionToggle(false);
                currentTarget = null;
            }
            viewier?.ToggleCursorSelection(false);
        }
    }
}
