using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ASUL.Console
{
    public class CommandBuffer : MonoBehaviour
    {
        private List<CommandData> commandHistory = new List<CommandData>();
        private int currentIndex = -1;

        private void Awake()
        {
            Load();
        }

        public void Save(string Text)
        {
            if (string.IsNullOrEmpty(Text)) return;

            FindSameCommandsAndClear();

            if (commandHistory.Count == 0 || commandHistory.Last().command != Text)
            {
                commandHistory.Add(new CommandData { command = Text });
            }
            currentIndex = commandHistory.Count;

            string folderPath = Path.Combine(Application.persistentDataPath, "Console");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, "CommandBuffer.json");

            try
            {
                string json = JsonConvert.SerializeObject(commandHistory);
                File.WriteAllText(filePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError("Ошибка сохранения истории: " + e.Message);
            }
        }

        private void Load()
        {
            string folderPath = Path.Combine(Application.persistentDataPath, "Console");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, "CommandBuffer.json");

            if (!File.Exists(filePath))
            {
                commandHistory = new List<CommandData>();
                currentIndex = 0;
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(json))
                {
                    commandHistory = JsonConvert.DeserializeObject<List<CommandData>>(json) ?? new List<CommandData>();
                }
                currentIndex = commandHistory.Count;
            }
            catch (Exception e)
            {
                Debug.LogError("Ошибка загрузки истории: " + e.Message);
                commandHistory = new List<CommandData>();
                currentIndex = 0;
            }

            ClearAllSpam();
        }

        public string UpArrow()
        {
            if (commandHistory.Count == 0) return "";
            currentIndex = Mathf.Max(0, currentIndex - 1);
            return commandHistory[currentIndex].command;
        }

        public string DownArrow()
        {
            if (commandHistory.Count == 0) return "";
            currentIndex = Mathf.Min(commandHistory.Count, currentIndex + 1);
            if (currentIndex == commandHistory.Count) return "";
            return commandHistory[currentIndex].command;
        }

        private void FindSameCommandsAndClear()
        {
            commandHistory = commandHistory
                .GroupBy(cmd => cmd.command)
                .Select(group => group.Last())
                .ToList();

            currentIndex = commandHistory.Count;
        }

        private void ClearAllSpam()
        {
            var toRemove = new List<CommandData>();

            foreach (var command in commandHistory)
            {
                if (!CommandDatabaseSample.IsCommandList(command.command))
                {
                    toRemove.Add(command);
                }
            }

            foreach (var item in toRemove)
            {
                commandHistory.Remove(item);
            }
        }
    }

    [Serializable]
    public class CommandData
    {
        public string command;
    }
}