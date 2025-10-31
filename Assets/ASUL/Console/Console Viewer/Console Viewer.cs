using ASUL.InputSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ASUL.Console
{
    public class ConsoleViewer : MonoBehaviour
    {

        public static Action<string> OnConsoleLog;
        public static Action<string> OnConsoleLogError;

        #region Ёлементы интерфейса

        [SerializeField] private TMP_InputField _commandLine;

        [SerializeField] private TextMeshProUGUI _consoleLogs;

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
            _commandLine.onValidateInput += (text, charIndex, addedChar) =>
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
                _commandLine.Select();
                _commandLine.ActivateInputField();
            }
            else
            {
                _commandLine.DeactivateInputField();
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }

        private void OnEnterPress(InputAction.CallbackContext ctx)
        {
            CommandProcessor.OnSendCommand?.Invoke(_commandLine.text);
            _commandLine.text = string.Empty;
            SelectCommandLine();
        }

        public void ClearConsole()
        {
            _consoleLogs.text = string.Empty;
            SelectCommandLine();
        }

        private void Log(string message)
        {
            _consoleLogs.text += message + "\n";  
        }

        private void LogError(string message)
        {
            _consoleLogs.text += $"<color=red>{message}</color>" + "\n";
        }

        private void OpenTutorial(InputAction.CallbackContext ctx)
        {
            WebsiteLauncher.LaunchWebsiteStatic("https://coherent-manatee-f2f.notion.site/Console-manual-29cb9c25bcc28065bbded5b8562be31a");
        }

        private void SelectCommandLine()
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(_commandLine.gameObject);
            _commandLine.ActivateInputField();
        }

    }
}

