using ASUL.NPC.PlayerDetection;
using System;
using Unity.VisualScripting;
using UnityEngine;


namespace ASUL.Console
{
    public class DieplomeCommandDatabase : CommandDatabase
    {

        private protected override void Awake()
        {
            SpecializedCommandDictionary =
            new()
            {
                {"setavailable", SetAvailable},
                {"setpreset", SetPreset},
                {"setvisible", SetVisible}
            };

            base.Awake();
        }


        #region Первичный параметр


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

        #endregion
    }
}
