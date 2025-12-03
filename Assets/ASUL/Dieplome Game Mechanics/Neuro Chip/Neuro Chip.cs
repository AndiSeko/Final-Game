using ASUL.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

namespace ASUL.Dieplome.NeuroChip
{
    public class NeuroChip : MonoBehaviour
    {
        [SerializeField] private NeuroChipViewier viewier;

        private void OnEnable()
        {
            InputManager.Instance.actions.Player.Neurochip.performed += View;
            InputManager.Instance.actions.Player.Neurochip.canceled += Unview;
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.actions.Player.Neurochip.performed -= View;
                InputManager.Instance.actions.Player.Neurochip.canceled -= Unview;
            }
        }

        private void View(InputAction.CallbackContext ctx)
        {
            viewier.IsView = true;

            ToggleVisible(true);
        }

        private void Unview(InputAction.CallbackContext ctx)
        {
            viewier.IsView = false;

            ToggleVisible(false);
        }


        private void ToggleVisible(bool value)
        {
            NeuroChipVisible[] visibles = FindObjectsByType<NeuroChipVisible>(FindObjectsSortMode.None);

            foreach (NeuroChipVisible visible in visibles)
            {
                visible.ToggleVisible(value);
            }
        }

    }
}