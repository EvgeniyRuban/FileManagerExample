using FileManagerExample.Models.Operations;
using System;

namespace FileManagerExample;

public static class OperationAnalizer
{
    private static List<Operation> _operations = new List<Operation>()
    {
        new ChangeCurrentDirectoryOperation(),
        new MakeDirectoryOperation(),
        new ListDirectoryContentOperation(),
    };

    // Подумать над названием.
    // Нужно возвращать модель с полным отчетом о попытке проанализировать строку.
    public static Operation? GetOperation(string text)
    {
        // Что если название директории будет составное?
        return FindOperationByCommand(text);
    }
    
    private static Operation? FindOperationByCommand(string text)
    {
        var array = text.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        foreach (var operation in _operations)
        {
            (int MinIndex, int MaxIndex) commandPossibleRange = GetPossibleCommandRange(operation);

            if (array.Length <= commandPossibleRange.MinIndex)
            {
                continue;
            }

            for (int i = commandPossibleRange.MinIndex; i <= commandPossibleRange.MaxIndex; i++)
            {
                if (operation.Command.Designations.Any(d => d == array[i]))
                {
                    return operation;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// The possible range of a command in a mask is the range between the minimum command index in the absence 
    /// of all optional components (before the command) and the maximum, which is the totality of all components (before the command).
    /// The method calculate and returns this range.
    /// </summary>
    private static (int MinIndex, int MaxIndex) GetPossibleCommandRange(Operation operation)
    {
        int maxIndex = IndexOfCommand(operation);
        int requiredComponentCountBeforeCommand = GetComponentCountByRequiredFlagValue(operation, true, maxIndex);
        return (requiredComponentCountBeforeCommand, maxIndex);
    }

    private static int IndexOfCommand(Operation operation) => operation.MaskComponents.IndexOf(operation.Command);

    /// <summary>
    /// The method counts the number of mask components whose Required property value is 
    /// <paramref name="requiredFlagValue"/> up to the specified <paramref name="lastIndex"/>.
    /// </summary>
    private static int GetComponentCountByRequiredFlagValue(Operation operation, bool requiredFlagValue, int lastIndex)
    {
        int componentCount = 0;

        for (int i = 0; i < lastIndex; i++)
        {
            if (operation.MaskComponents[i].Required)
            {
                componentCount++;
            }
        }
        return componentCount;
    }
}