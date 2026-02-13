using ASUL.InputSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ASUL.Console
{
    public class CommandDatabase : MonoBehaviour
    {
        public static CommandDatabase CurrentDatabase { get; private protected set; }

        public static Action<Command> CommandProcess;


        private Dictionary<string, Action<Command>> CommandDictionary;

        private protected Dictionary<string, Action<Command>> SpecializedCommandDictionary;



        #region Базовые Unity-методы

        private protected virtual void Awake()
        {
            if (CurrentDatabase == null)
            {
                CurrentDatabase = this;
            }
            else
            {
                ConsoleViewer.OnConsoleLogError?.Invoke("There are more command databases than you need!"); return;
            }

            CommandDictionary =
            new()
            {
                {"info", (cmd) => Info()},
                {"clear", (cmd) => Clear()}
            };

            if (SpecializedCommandDictionary != null)
            {
                foreach (var key in SpecializedCommandDictionary.Keys)
                {
                    CommandDictionary.Add(key, SpecializedCommandDictionary[key]);
                }
            }
        }

        private protected virtual void OnEnable()
        {
            CommandProcess += FindAndPerform;
        }

        private protected virtual void OnDisable()
        {
            CommandProcess -= FindAndPerform;
        }

        #endregion

        public void FindAndPerform(Command command)
        {
            try
            {
                CommandDictionary[command.primaryParameter]?.Invoke(command);
            }
            catch
            {
                ConsoleViewer.OnConsoleLogError?.Invoke("Command error: invalid syntax!");
            }
        }

        public bool IsCommandList(string commands)
        {
            Command[] c = CommandReader.ReadCommands(commands);

            foreach (Command command in c)
            {
                foreach (var key in CommandDictionary.Keys)
                {
                    if (command.primaryParameter == key)
                        return true;
                }
            }
            return false;
        }


        #region Первичный параметр

        private void Info()
        {
            string dataPath = Application.dataPath.Replace("\\", "/");
            string persistentPath = Application.persistentDataPath.Replace("\\", "/");

            ConsoleViewer.OnConsoleLog?.Invoke("<style=Console><color=#36CF8C>Console v 2.0</color></style>" +
                "\nCreated by Andi Seko" +
                "\n© ASUL 2025" +
                "\nFor additional information, read the documentation by pressing the f1 key." +
                "\n\n<style=Miku>Game Info</style>" +
                $"\nApplication Name: {Application.productName}" +
                $"\nVersion: {Application.version}" +
                $"\nPlatform: {Application.platform}" +
                $"\nCreator: {Application.companyName}" +
                $"\nData path: <link=\"folder:{dataPath}\">{dataPath}</link>" +
                $"\nPersistent data path: <link=\"folder:{persistentPath}\">{persistentPath}</link>" +
                $"\nUnity version: {Application.unityVersion}");
        }

        private void Clear()
        {
            ConsoleViewer[] viewers = FindObjectsByType<ConsoleViewer>(FindObjectsSortMode.None);

            foreach (ConsoleViewer viewer in viewers)
            {
                viewer.ClearConsole();
            }
        }

        private void InputMaps()
        {
            foreach (string map in InputManager.Instance.GetAvailableMaps())
            {
                if (map == InputManager.Instance.CurrentState)
                {
                    ConsoleViewer.OnConsoleLog?.Invoke($"<style=Miku>{map}</style>");
                    continue;
                }
                ConsoleViewer.OnConsoleLog?.Invoke($"{map}");
            } 
        }

        #endregion
    }
}