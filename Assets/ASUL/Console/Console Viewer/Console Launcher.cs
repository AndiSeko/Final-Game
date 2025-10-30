using ASUL.Canvas;
using ASUL.InputSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace ASUL.Console
{
    public class ConsoleLauncher : CanvasGroupLauncher
    {
        private void OnEnable()
        {
            InputManager.Instance.actions.Player.Console.performed += Open;
            InputManager.Instance.actions.UI.Console.performed += Open;
            InputManager.Instance.actions.Console.Back.performed += Close;
        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.actions.Player.Console.performed -= Open;
                InputManager.Instance.actions.UI.Console.performed -= Open;
                InputManager.Instance.actions.Console.Back.performed -= Close;
            }
        }

        protected override void Open(InputAction.CallbackContext ctx)
        {
            base.Open(ctx);
            InputManager.Instance.ChangeInputMap("Console", CursorLockMode.None);
        }

        protected override void Close(InputAction.CallbackContext ctx)
        {
            base.Close(ctx);
            InputManager.Instance.RevertToPreviousMap();
        }


    }
}
