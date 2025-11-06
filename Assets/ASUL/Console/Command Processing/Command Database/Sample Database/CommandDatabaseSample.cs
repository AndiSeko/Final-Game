using System;
using Unity.VisualScripting;
using UnityEngine;


namespace ASUL.Console
{
    public class CommandDatabaseSample : CommandDatabase
    {
        public static Action<Command> OnCommandProcess;

        private void OnEnable()
        {
            OnCommandProcess += FindAndPerform;
        }

        private void OnDisable()
        {
            OnCommandProcess -= FindAndPerform;
        }

        public override void FindAndPerform(Command command)
        {
            switch (command.primaryParameter)
            {
                case "info": Info(); break;
                case "setawaible": SetAwaible(command); break;

                default: ConsoleViewer.OnConsoleLogError?.Invoke("Command error: invalid syntax!"); break;
            }
        }

        #region Первичный параметр

        private void Info()
        {
            ConsoleViewer.OnConsoleLog?.Invoke("<style=Console><color=#36CF8C>Console v 2.0</color></style>" +
                "\nCreated by Andi Seko" +
                "\n© ASUL 2025" +
                "\nFor additional information, read the documentation by pressing the f1 key." +
                "\n\n<style=Miku>Game Info</style>" +
                $"\nApplication Name: {Application.productName}" +
                $"\nVersion: {Application.version}" +
                $"\nPlatform: {Application.platform}" +
                $"\nCreator: {Application.companyName}" +
                $"\nData path: {Application.dataPath}" +
                $"\nUnity version: {Application.unityVersion}");
        }

        private void SetAwaible(Command command)
        {
            bool commandValue;

            switch (command.value)
            {
                case "true": case "1": commandValue = true; break;
                case "false": case "0": commandValue = false; break;
                default: ConsoleViewer.OnConsoleLogError?.Invoke("Command error: invalid value, use: 1/0 or true/false!"); return;
            }

            switch (command.secondaryParameter)
            {
                case "interact": CSetAwaible.Interact(commandValue); break;
                case "walk": CSetAwaible.Walk(commandValue); break;
                case "run": CSetAwaible.Run(commandValue); break;
                case "jump": CSetAwaible.Jump(commandValue); break;
                default: ConsoleViewer.OnConsoleLogError?.Invoke("Command error: secondary parameter is invalid!"); return;
            }

        }

        #endregion



    }
}
