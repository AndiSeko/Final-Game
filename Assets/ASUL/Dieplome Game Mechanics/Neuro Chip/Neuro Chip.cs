using ASUL.InputSystem;
using ASUL.NPC.PlayerDetection;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ASUL.Dieplome.NeuroChip
{
    public class NeuroChip : MonoBehaviour
    {
        [SerializeField] private NeuroChipViewer viewier;

        private float charge = 1f;

        private PlayerVisibilityState lastState;

        public static Action<float> OnChargeChanged;

        private void OnEnable()
        {
            InputManager.Instance.actions.Player.Neurochip.performed += View;
            InputManager.Instance.actions.Player.Neurochip.canceled += Unview;
            OnChargeChanged += ChangeCharge;

            viewier.SetChargeView(1);
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.actions.Player.Neurochip.performed -= View;
                InputManager.Instance.actions.Player.Neurochip.canceled -= Unview;
            }
            OnChargeChanged -= ChangeCharge;
        }

        private void ChangeCharge(float value)
        {
            float v = value / 100f;

            if (charge + v <= 0)
            {
                charge = 0;

                PlayerVisibilityManager.PlayerVisibilityState = lastState;
            }
            else
            {
                charge += v;
            }

            viewier.SetChargeView(charge);

        }

        private void View(InputAction.CallbackContext ctx)
        {
            viewier.IsView = true;
            viewier.ToggleOpen(true);

            ToggleVisible(true);

            if (charge > 0)
            {
                lastState = PlayerVisibilityManager.PlayerVisibilityState;
                PlayerVisibilityManager.PlayerVisibilityState = PlayerVisibilityState.NeurochipInvisible;
            }
        }

        private void Unview(InputAction.CallbackContext ctx)
        {
            viewier.IsView = false;
            viewier.ToggleOpen(false);

            ToggleVisible(false);

            PlayerVisibilityManager.PlayerVisibilityState = lastState;
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