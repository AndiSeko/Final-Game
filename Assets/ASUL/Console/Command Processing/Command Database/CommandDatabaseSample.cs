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
                case "status": Status(); break;
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

        private void Status()
        {
            ConsoleViewer.OnConsoleLog?.Invoke("<style=Miku>Game Status</style>");
        }

        #endregion


    }
}
