using ASUL.InputSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace ASUL.Console
{
    public class ConsoleViewer : MonoBehaviour
    {

        public static Action<string> OnConsoleLog;
        public static Action<string> OnConsoleLogError;

        #region Ёлементы интерфейса

        [SerializeField] private TMP_InputField commandLine;

        [SerializeField] private TextMeshProUGUI consoleLogs;

        [SerializeField] private ScrollRect logsScrollRect;

        #endregion

        public bool IsEnable { set { SetEnable(value); } }

        private void OnEnable()
        {
            InputManager.Instance.actions.Console.Enter.performed += OnEnterPress;
            InputManager.Instance.actions.Console.Tutotial.performed += OpenTutorial;
            OnConsoleLog += Log;
            OnConsoleLogError += LogError;

        }

        private void OnDisable()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.actions.Console.Enter.performed -= OnEnterPress;
                InputManager.Instance.actions.Console.Tutotial.performed -= OpenTutorial;
            }
            OnConsoleLog -= Log;
            OnConsoleLogError -= LogError;
        }

        private void Awake()
        {
            commandLine.onValidateInput += (text, charIndex, addedChar) =>
            {
                if (addedChar == '`' || addedChar == 'Є')
                {
                    return '\0';
                }
                return addedChar;
            };
        }


        private void SetEnable(bool enable)
        {
            if (enable)
            {
                commandLine.Select();
                commandLine.ActivateInputField();
            }
            else
            {
                commandLine.DeactivateInputField();
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private void OnEnterPress(InputAction.CallbackContext ctx)
        {
            CommandProcessor.OnSendCommand?.Invoke(commandLine.text);
            commandLine.text = string.Empty;
            ScrollToTheEnd();
            SelectCommandLine();
        }

        public void ClearConsole()
        {
            consoleLogs.text = string.Empty;
            SelectCommandLine();
        }

        private void Log(string message)
        {
            consoleLogs.text += message + "\n";  
        }

        private void LogError(string message)
        {
            consoleLogs.text += $"<color=red>{message}</color>" + "\n";
        }

        private void OpenTutorial(InputAction.CallbackContext ctx)
        {
            WebsiteLauncher.LaunchWebsiteStatic("https://coherent-manatee-f2f.notion.site/Console-manual-29cb9c25bcc28065bbded5b8562be31a");
        }

        private void SelectCommandLine()
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(commandLine.gameObject);
            commandLine.ActivateInputField();
        }

        private void ScrollToTheEnd()
        {
            UnityEngine.Canvas.ForceUpdateCanvases();
            logsScrollRect.verticalNormalizedPosition = 0f;
        }

    }
}

