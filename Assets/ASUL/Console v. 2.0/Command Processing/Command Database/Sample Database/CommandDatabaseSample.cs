using ASUL.NPC.PlayerDetection;
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
                case "setavailable": SetAvailable(command); break;
                case "setpreset": SetPreset(command); break;
                case "setvisible": SetVisible(command); break;
                case "clear": Clear(); break;

                default: ConsoleViewer.OnConsoleLogError?.Invoke("Command error: invalid syntax!"); break;
            }
        }

        public static bool IsCommandList(string commands)
        {
            Command[] c = CommandReader.ReadCommands(commands);

            foreach (Command command in c)
            {
                switch (command.primaryParameter)
                {
                    case "info":
                    case "setavailable":
                    case "setpreset":
                    case "setvisible": break;
                    default: return false;
                }
            }
            return true;
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

        private void SetAvailable(Command command)
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
                case "interact": CSetAvailable.Interact(commandValue); break;
                case "walk": CSetAvailable.Walk(commandValue); break;
                case "run": CSetAvailable.Run(commandValue); break;
                case "jump": CSetAvailable.Jump(commandValue); break;
                case "look": CSetAvailable.Look(commandValue); break;
                default: ConsoleViewer.OnConsoleLogError?.Invoke("Command error: secondary parameter is invalid!"); return;
            }

        }

        private void SetPreset(Command command)
        {
            switch (command.secondaryParameter)
            {
                case "play": CSetPreset.Play(); break;
                case "sprint": CSetPreset.Sprint(); break;
                case "look": CSetPreset.Look(); break;
                case "jump": CSetPreset.Jump(); break;
                case "cutscene": CSetPreset.Cutscene(); break;
                case "full": CSetPreset.Full(); break;

                default: ConsoleViewer.OnConsoleLogError?.Invoke("Command error: secondary parameter is invalid!"); return;
            }
        }

        private void SetVisible(Command command)
        {
            switch (command.secondaryParameter)
            {
                case "true": case "1":
                    PlayerVisibilityManager.PlayerVisibilityState = PlayerVisibilityState.Default; break;
                case "false": case "0":
                    PlayerVisibilityManager.PlayerVisibilityState = PlayerVisibilityState.ConsoleInvisible; break;

                default: ConsoleViewer.OnConsoleLogError?.Invoke("Command error: secondary parameter is invalid!"); return;
            }
            ConsoleViewer.OnConsoleLog?.Invoke($"The visibility has been successfully changed to {command.secondaryParameter}.");
        }

        private void Clear()
        {
            ConsoleViewer[] viewers = FindObjectsByType<ConsoleViewer>(FindObjectsSortMode.None);

            foreach (ConsoleViewer viewer in viewers)
            {
                viewer.ClearConsole();
            }
        }

        #endregion
    }
}
