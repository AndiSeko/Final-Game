using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ASUL.Console
{
    public class ConsoleViewer : MonoBehaviour
    {

        #region Ёлементы интерфейса

        [SerializeField] private TMP_InputField _commandLine;

        [SerializeField] private TextMeshProUGUI _consoleLogs;

        #endregion

        public bool IsEnable { set { SetEnable(value); } }


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

    }
}

