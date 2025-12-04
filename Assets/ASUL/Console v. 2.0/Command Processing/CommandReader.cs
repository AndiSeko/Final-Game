using System.Collections.Generic;

namespace ASUL.Console
{
    public static class CommandReader
    {
        public static Command[] ReadCommands(string sourceText)
        {
            string[] commands = SplitCommands(sourceText);

            List<Command> result = new();

            for (int i = 0; i < commands.Length; i++ )
            {
                result.Add(ConvertSourceTextToCommandArray(commands[i]));
            }

            result.RemoveAll(c => CommandIsEmpty(c));

            return result.ToArray();

        }

        public static Command ConvertSourceTextToCommandArray(string sourceText)
        {
            string[] convertedText = ConvertSourceTextToTextArray(sourceText);

            return convertedText.Length switch
            {
                1 => new Command(convertedText[0]),
                2 => new Command(convertedText[0], convertedText[1]),
                3 => new Command(convertedText[0], convertedText[1], convertedText[2]),
                _ => new("", "", "")
            };
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

        public static bool CommandIsEmpty(Command command)
        {
            return string.IsNullOrEmpty(command.primaryParameter) && string.IsNullOrEmpty(command.secondaryParameter) && string.IsNullOrEmpty(command.value);
        }
    }
}
