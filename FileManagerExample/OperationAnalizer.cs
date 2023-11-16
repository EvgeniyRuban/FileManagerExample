using FileManagerExample.Models.Operations;
using System.Text;

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
    public static Operation? GetOperation(string operationText, string currentDirectoryPath)
    {
        var array = operationText.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        CheckAndUnionAbsolutePath(ref array);
        CheckAndUnionRelativePath(ref array, currentDirectoryPath);
        var operation = FindOperationByCommand(array);
        return operation;
    }

    /// <summary>
    /// The method checks for the presence of substrings of a compound absolute path in the refferenced <paramref name="array"/> and, if one is detected, combines it.
    /// </summary>
    private static void CheckAndUnionAbsolutePath(ref string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (Path.IsPathRooted(array[i]))
            {
                int pathElementCount = 1;
                var possiblePath = new StringBuilder(array[i]);

                for (int j = i + 1; j < array.Length; j++)
                {
                    possiblePath.Append(' ').Append(array[j]);

                    if (File.Exists(possiblePath.ToString()) || Directory.Exists(possiblePath.ToString()))
                    {
                        pathElementCount = j - i + 1;
                    }
                }

                if (pathElementCount > 1)
                {
                    UnionArrayPart(ref array, i, pathElementCount);
                }
            }
        }
    }

    /// <summary>
    /// The method checks for the presence of substrings of a compound relative path in the refferenced <paramref name="array"/> and, if one is detected, combines it.
    /// </summary>
    private static void CheckAndUnionRelativePath(ref string[] array, string currentDirectoryPath)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int pathElementCount = 0;
            var possiblePath = new StringBuilder(currentDirectoryPath)
                .Append(Path.DirectorySeparatorChar);

            for (int j = i; j < array.Length; j++)
            {
                if (j == i)
                {
                    possiblePath.Append(array[j]);
                }
                else
                {
                    possiblePath.Append(' ').Append(array[j]);
                }

                if (File.Exists(possiblePath.ToString()) || Directory.Exists(possiblePath.ToString()))
                {
                    pathElementCount = j - i + 1;
                }
            }

            if (pathElementCount > 1)
            {
                UnionArrayPart(ref array, i, pathElementCount);
            }
        }
    }

    private static Operation? FindOperationByCommand(string[] array)
    {
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

    /// <summary>
    /// The method for union of the selected part of the <paramref name="array"/> starts with <paramref name="unionAreaStartIndex"/>.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"></exception>
    private static void UnionArrayPart(ref string[] array, int unionAreaStartIndex, int unionAreaLength)
    {
        int newArrayLength = array.Length - unionAreaLength + 1;
        var newArray = new string[newArrayLength];
        var unionArea = new StringBuilder();
        int unionAreaLastIndex = unionAreaStartIndex + unionAreaLength - 1;

        for (int i = unionAreaStartIndex; i <= unionAreaLastIndex; i++)
        {
            unionArea.Append(array[i]).Append(' ');
        }

        for (int newArrayIterator = 0, oldArrayIterator = 0; newArrayIterator < newArray.Length; newArrayIterator++, oldArrayIterator++)
        {
            if (newArrayIterator == unionAreaStartIndex)
            {
                newArray[newArrayIterator] = unionArea.ToString();
                oldArrayIterator += unionAreaLength - 1;
            }
            else
            {
                newArray[newArrayIterator] = array[oldArrayIterator];
            }
        }

        array = newArray;
    }
}