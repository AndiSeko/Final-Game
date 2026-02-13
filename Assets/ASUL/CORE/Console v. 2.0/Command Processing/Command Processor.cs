using ASUL.Console;
using ASUL.InputSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ASUL.Console
{
    public class CommandProcessor : MonoBehaviour
    {
        public static Action<string> OnSendCommand;

        public Command[] commands;

        private void OnEnable()
        {
            OnSendCommand += Process;
        }

        private void OnDisable()
        {
            OnSendCommand -= Process;
        }


        private void Process(string sourceText)
        {
            commands = CommandReader.ReadCommands(sourceText);

            foreach (Command command in commands)
            {
                CommandDatabase.CommandProcess(command);
            }
        }

    }
}
