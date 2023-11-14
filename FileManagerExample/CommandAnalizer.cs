using FileManagerExample.Models.Commands;

namespace FileManagerExample;

public static class CommandAnalizer
{
    private static List<Command> _commands = new List<Command>()
        {
            new ChangeDirectoryCommand()
        };

    // Подумать над названием.
    public static Command Analize(string text)
    {
        // Что если название директории будет составное?
        return FindByCommandKeyword(text);
    }

    // Нужно возвращать модель с полным отчетом о попытке проанализировать строку.
    private static Command? FindByCommandKeyword(string text)
    {
        var array = text.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        foreach (var command in _commands)
        {
            int commandKeywordIndex = command.MaskComponents.IndexOf(CommandMaskComponent.Command);

            // Обработать с учетом необязательных модификаторов.
            if (array.Length <= commandKeywordIndex)
            {
                continue;
            }

            if (command.Designations.Any(d => d == array[commandKeywordIndex]))
            {
                return command;
            }
        }

        return null;
    }
}