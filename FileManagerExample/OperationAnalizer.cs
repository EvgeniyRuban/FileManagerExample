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
        CheckAbsolutePath(ref array);
        CheckRelativePath(ref array, currentDirectoryPath);
        return FindOperationByCommand(array);
    }

    // Требуется рефакторинг!!! Подумать над названием
    private static void CheckAbsolutePath(ref string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int pathElementCount = 0;
            int startPathIndex = i;

            // Если подстрока имеет корень пути.
            if (Path.IsPathRooted(array[i]))
            {
                pathElementCount = 1;
                var tempResult = array[i];

                // Проверяем ситуацию в которой путь может быть составной (из нескольких подстрок).
                for (int j = i + 1; j < array.Length; j++)
                {
                    tempResult += $" {array[j]}";
                    bool fileExists = File.Exists(tempResult);
                    bool directoryExists = Directory.Exists(tempResult);
                    File.Exists(tempResult);
                    if (fileExists || directoryExists)
                    {
                        pathElementCount = j - i + 1;
                    }
                }

                // Если путь составной, тогда производим склейку.
                if (pathElementCount != 0)
                {
                    int newArrayLength = array.Length - pathElementCount + 1;
                    string[] newArray = new string[newArrayLength];
                    var combinedPath = new StringBuilder();
                    int lastPathIndex = startPathIndex + pathElementCount - 1;

                    // Склеиваем найденный путь.
                    for (int k = startPathIndex; k <= lastPathIndex; k++)
                    {
                        combinedPath.Append(array[k]);
                        combinedPath.Append(" ");
                    }

                    // Создаем массив и переписываем в него данные из старого, с учетом новой размерности.
                    for (int k = 0, x = 0; k < newArray.Length; k++, x++)
                    {
                        if (k == startPathIndex)
                        {
                            newArray[k] = combinedPath.ToString();
                            x += pathElementCount - 1;
                        }
                        else
                        {
                            newArray[k] = array[x];
                        }
                    }

                    array = newArray;
                }
            }
        }
    }

    // Требуется рефакторинг!!! Подумать над названием
    private static void CheckRelativePath(ref string[] array, string currentDirectoryPath)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int pathElementCount = 0;
            var possiblePath = new StringBuilder(currentDirectoryPath + Path.DirectorySeparatorChar);

            for (int j = i; j < array.Length; j++)
            {
                if (j == i)
                {
                    possiblePath.Append(array[j]);
                }
                else
                {
                    possiblePath.Append(string.Concat(" ", array[j]));
                }

                // Проверяем, есть ли по полученому абсолутмому пути доступный файл или папка.
                if (File.Exists(possiblePath.ToString()) || Directory.Exists(possiblePath.ToString()))
                {
                    pathElementCount = j - i + 1;
                }
            }

            if (pathElementCount > 1) // Если отнасительный путь составной, склеиваем его.
            {
                var combinedPath = new StringBuilder();
                int newArrayLength = array.Length - pathElementCount + 1;
                var newArray = new string[newArrayLength];
                int lastPathIndex = i + pathElementCount - 1;

                // Склеиваем найденный путь.
                for (int j = i; j <= lastPathIndex; j++)
                {
                    combinedPath.Append(array[j]);
                    combinedPath.Append(" ");
                }

                // Создаем массив и переписываем в него данные из старого, с учетом новой размерности.
                for (int j = 0, k = 0; j < newArray.Length; j++, k++)
                {
                    if (j == i)
                    {
                        newArray[j] = combinedPath.ToString();
                        k += pathElementCount - 1;
                    }
                    else
                    {
                        newArray[j] = array[k];
                    }
                }

                array = newArray;
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
}