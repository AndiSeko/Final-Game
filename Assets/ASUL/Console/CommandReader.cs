using UnityEngine;

namespace ASUL.Console
{
    public static class CommandReader
    {
        public static Command[] ReadCommands(string sourceText)
        {
            string[] commands = SplitCommands(sourceText);

            Command[] result = new Command[commands.Length];

            for (int i = 0; i < commands.Length; i++ )
            {
                result[i] = ConvertSourceTextToCommandArray(commands[i]);
            }

            return result;
        }

        public static Command ConvertSourceTextToCommandArray(string sourceText)
        {
            string[] convertedText = ConvertSourceTextToTextArray(sourceText);

            Command command = null;

            if (convertedText.Length == 3)
            {
                command = new(convertedText[0], convertedText[1], convertedText[2]);
            }

            return command;
        }

        public static string[] ConvertSourceTextToTextArray(string sourceText)
        {
            string formatedText = sourceText.Trim().ToLower();

            string[] commandSyntax = formatedText.Split(" ");

            return commandSyntax;
        }

        public static string[] SplitCommands(string commands)
        {
            string formatedText = commands.Trim().ToLower();

            string[] commandSyntax = formatedText.Split(";");

            return commandSyntax;
        }
    }
}
