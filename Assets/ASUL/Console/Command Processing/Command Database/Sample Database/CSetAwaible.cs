using ASUL.FPC;
using ASUL.Interactions;
using UnityEngine;

namespace ASUL.Console
{
    public class CSetAwaible : MonoBehaviour
    {
        public static void Interact(bool value)
        {
            PlayerInteractor[] interactors = FindObjectsByType<PlayerInteractor>(FindObjectsSortMode.None);

            foreach (PlayerInteractor interactor in interactors)
            {
                interactor.IsAwaible = value;
            }

            PlayerInteractorViewier[] viewiers = FindObjectsByType<PlayerInteractorViewier>(FindObjectsSortMode.None);

            foreach (var viewier in viewiers)
            {
                viewier.ToggleOpen(value);
            }

            ConsoleViewer.OnConsoleLog?.Invoke($"The interaction availability has been successfully changed to {value}.");
        }
        
        public static void Walk(bool value)
        {
            FirstPersonController[] controllers = FindObjectsByType<FirstPersonController>(FindObjectsSortMode.None);

            foreach (FirstPersonController controller in controllers)
            {
                controller.isCanMove = value;
            }

            ConsoleViewer.OnConsoleLog?.Invoke($"The walking availability has been successfully changed to {value}.");
        }

        public static void Run(bool value)
        {
            FirstPersonController[] controllers = FindObjectsByType<FirstPersonController>(FindObjectsSortMode.None);

            foreach (FirstPersonController controller in controllers)
            {
                controller.isCanSprint = value;
            }

            ConsoleViewer.OnConsoleLog?.Invoke($"The running availability has been successfully changed to {value}.");
        }

        public static void Jump(bool value)
        {
            FirstPersonController[] controllers = FindObjectsByType<FirstPersonController>(FindObjectsSortMode.None);

            foreach (FirstPersonController controller in controllers)
            {
                controller.isCanJump = value;
            }

            ConsoleViewer.OnConsoleLog?.Invoke($"The walking availability has been successfully changed to {value}.");
        }

        public static void Look(bool value)
        {
            FirstPersonController[] controllers = FindObjectsByType<FirstPersonController>(FindObjectsSortMode.None);

            foreach (FirstPersonController controller in controllers)
            {
                controller.isCanRotateCamera = value;
            }

            ConsoleViewer.OnConsoleLog?.Invoke($"The looking availability has been successfully changed to {value}.");
        }

    }

}