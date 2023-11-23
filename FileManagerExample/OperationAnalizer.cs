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
        new RenameFileOrDirectoryTitleOperation(),
        new RemoveFileOrDirectoryOperation(),
        new CreateMultipleFilesOperation(),
        new ShowFileContentOperation(),
        new AppendTextToFileOperation(),
        new CopyFileOrDirectoryOperation(),
        new ClearConsoleOperation(),
        new HelpOperation(),
    };
    private static List<OperationConstArg> _constArgs = new List<OperationConstArg>()
    {
        new CurrentDirectoryArg(),
        new ParentDirectoryArg(),
        new ParentOfParentDirectoryArg(),
    };

    public static OperationInfo GetOperationAnalysis(string operationText, string currentDirectoryPath)
    {
        var operationAnalysisInfo = new OperationInfo();
        var array = operationText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        ReplaceConstantArgs(ref array, currentDirectoryPath);

        JoinParameters(ref array);

        var operation = FindOperationByCommand(array);

        if (operation is null)
        {
            operationAnalysisInfo.ErrorInfo = OperationAnalysisErrorDescriptions.OperationUndefined;
            return operationAnalysisInfo;
        }

        operationAnalysisInfo.OperationDetected = true;
        operationAnalysisInfo.OperationType = operation.Type;

        if (!CheckByRequiredMaskComponentCount(operation, array.Length))
        {
            operationAnalysisInfo.ErrorInfo = OperationAnalysisErrorDescriptions.IncorrectMaskComponentCount;
            return operationAnalysisInfo;
        }

        var interpretedComponents = MatchMask(operation, array);

        if (interpretedComponents is null)
        {
            operationAnalysisInfo.ErrorInfo = OperationAnalysisErrorDescriptions.MaskMatchFailed;
            return operationAnalysisInfo;
        }

        foreach ( var component in interpretedComponents)
        {
            switch (component.ComponentType)
            {
                case OperationComponents.Parameter:
                    {
                        var argumentInfo = component as OperationArgInfo;
                        var temp = argumentInfo.Value.TrimStart(OperationMaskDefinitions.StartArgumentMarker);
                        argumentInfo.Value = temp.TrimEnd(OperationMaskDefinitions.EndArgumentMarker);
                        operationAnalysisInfo.Args.Add(argumentInfo);
                        break;
                    }
                case OperationComponents.Modifier: operationAnalysisInfo.Modifiers.Add((OperationModifierInfo)component); break;
            }
        }
        operationAnalysisInfo.Success = true;

        return operationAnalysisInfo;
    }

    private static ICollection<IOperationComponentInfo>? MatchMask(Operation operation, string[] array)
    {
        var interpretedComponents = new IOperationComponentInfo[array.Length];
        var mask = operation.MaskComponents;

        // Производим интерпритацию полученных компонентов из массива array в компоненты маски.
        for (int i = 0; i < array.Length; i++)
        {
            if (IsCommand(array[i], operation.Command))
            {
                interpretedComponents[i] = new OperationCommandInfo();
            }
            else if (IsParameter(array[i]))
            {
                interpretedComponents[i] = new OperationArgInfo();
            }
            else
            {
                foreach (var modifier in operation.Modifiers)
                {
                    if (IsModifier(array[i], modifier))
                    {
                        interpretedComponents[i] = new OperationModifierInfo(modifier.Assignment, modifier.Required);
                        break;
                    }
                }
            }
        }

        // Проверка на наличие компонентов которые остались неопределенными.
        if (interpretedComponents.Contains(null))
        {
            return null!;
        }

        // Проверка на наличие дубликатов модофикаторов.
        foreach (var modifier in operation.Modifiers)
        {
            if (HasModifierDuplicates(interpretedComponents, modifier))
            {
                return null!;
            }
        }

        // Проверка на наличие дубликатов команды.
        if (HasCommandDuplicates(interpretedComponents,  operation.Command))
        {
            return null!;
        }

        // Сопоставление компонентов маски операции с полученным набором компонентов прошедших интерпритацию.
        for (int i = 0, j = 0; i < mask.Count; i++)
        {
            bool bothComponentSameType = mask[i].ComponentType == interpretedComponents[j].ComponentType;
            if (bothComponentSameType)
            {
                if (mask[i].ComponentType == OperationComponents.Parameter)
                {
                    var parameter = mask[i] as OperationParameter;
                    interpretedComponents[j] = new OperationArgInfo(required: parameter.Required)
                    {
                        Value = array[j]
                    };
                }
                else if (mask[i].ComponentType == OperationComponents.Modifier)
                {
                    var maskModifier = mask[i] as OperationModifier;
                    var interpretedModifier = interpretedComponents[j] as OperationModifierInfo;
                    var bothModifierSameAssignment = maskModifier.Assignment == interpretedModifier.Assignment;

                    if (!bothModifierSameAssignment)
                    {
                        continue;
                    }
                }
                j++;
            }

            if (j == interpretedComponents.Length)
            {
                break;
            }

            if (i == mask.Count - 1 && j != interpretedComponents.Length)
            {
                return null!;
            }
        }

        return interpretedComponents;
    }

    private static bool HasCommandDuplicates(IEnumerable<IOperationComponentInfo> components, OperationCommand command) 
        => components.Count(c => c.ComponentType == OperationComponents.Command) > 1;

    private static bool HasModifierDuplicates(IEnumerable<IOperationComponentInfo> components, OperationModifier modifier)
    {
        int modifierCount = components
            .Where(c => c.ComponentType == OperationComponents.Modifier)
            .Count(m => ((OperationModifierInfo)m).Assignment == modifier.Assignment);

        return modifierCount > 1;
    }

    private static bool IsCommand(string text, OperationCommand command) => command.Declarations.Contains(text);

    private static bool IsParameter(string text)
        => text[0] == OperationMaskDefinitions.StartArgumentMarker && text[text.Length - 1] == OperationMaskDefinitions.EndArgumentMarker;

    private static bool IsModifier(string text, OperationModifier modifier) => modifier.Declaration == text;

    /// <summary>
    /// The method checks for the presence of a parameter by searching for markers of the beginning and end of the parameter; if the parameter is compound, it is joined.
    /// </summary>
    private static void JoinParameters(ref string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            char firstCharacter = array[i][0];
            if (firstCharacter == OperationMaskDefinitions.StartArgumentMarker)
            {
                int parameterElementCount = 1;

                for (int j = i; j < array.Length; j++)
                {
                    char lastCharacter = array[j][array[j].Length - 1];

                    if (i == j)
                    {
                        if(lastCharacter == OperationMaskDefinitions.EndArgumentMarker && array[j].Length != 1)
                        {
                            break;
                        }
                    }
                    else if (lastCharacter == OperationMaskDefinitions.EndArgumentMarker)
                    {
                        break;
                    }

                    if (j == array.Length - 1)
                    {
                        parameterElementCount = -1;
                    }

                    parameterElementCount++;
                }

                if (parameterElementCount > 1)
                {
                    Join(ref array, i, parameterElementCount, " ");
                }
            }
        }
    }

    private static Operation? FindOperationByCommand(string[] array)
    {
        foreach (var operation in _operations)
        {
            (int MinIndex, int MaxIndex) commandRange = operation.GetCommandPositionRangeInMask();

            if (array.Length <= commandRange.MinIndex)
            {
                continue;
            }

            for (int i = commandRange.MinIndex; i <= commandRange.MaxIndex; i++)
            {
                if (operation.Command.Declarations.Any(d => d == array[i]))
                {
                    return operation;
                }
            }
        }

        return null;
    }

    private static bool CheckByRequiredMaskComponentCount(Operation operation, int receivedComponentCount)
    {
        var requiredComponentCount = operation.MaskComponents.Count(c => c.Required);
        return receivedComponentCount >= requiredComponentCount && receivedComponentCount <= operation.MaskComponents.Count;
    }

    /// <summary>
    /// The method for union of the selected part of the <paramref name="array"/> starts with <paramref name="unionAreaStartIndex"/>.
    /// The <paramref name="joinString"/> is applied between all array components being joined
    /// </summary>
    /// <exception cref="IndexOutOfRangeException"></exception>
    private static void Join(ref string[] array, int unionAreaStartIndex, int unionAreaLength, string joinString = "")
    {
        int newArrayLength = array.Length - unionAreaLength + 1;
        var newArray = new string[newArrayLength];
        var unionArea = new StringBuilder();
        int unionAreaLastIndex = unionAreaStartIndex + unionAreaLength - 1;

        for (int i = unionAreaStartIndex; i <= unionAreaLastIndex; i++)
        {
            if (i == unionAreaLastIndex)
            {
                unionArea.Append(array[i]);
            }
            else
            {
                unionArea.Append(array[i]).Append(joinString);
            }
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

    private static void ReplaceConstantArgs(ref string[] array, string currentDirectoryPath)
    {
        for (int i = 0; i < array.Length; i++)
        {
            var currentString = array[i];
            var arg = _constArgs.FirstOrDefault(a => a.Declaration == currentString);

            if (arg != null)
            {
                var currentDirectoryInfo = new DirectoryInfo(currentDirectoryPath);
                switch (arg.Type)
                {
                    case OperationConstantArgTypes.CurrentDirectory:
                        {
                            array[i] = $"{OperationMaskDefinitions.StartArgumentMarker}" +
                                $"{currentDirectoryInfo.FullName}" +
                                $"{OperationMaskDefinitions.StartArgumentMarker}";
                            break;
                        }
                    case OperationConstantArgTypes.ParentDirectory:
                        {
                            array[i] = $"{OperationMaskDefinitions.StartArgumentMarker}" +
                                $"{currentDirectoryInfo.Parent?.FullName ?? currentDirectoryInfo.FullName}" +
                                $"{OperationMaskDefinitions.StartArgumentMarker}";
                            break;
                        }
                    case OperationConstantArgTypes.ParantOfParentDirectory:
                        {
                            array[i] = $"{OperationMaskDefinitions.StartArgumentMarker}" +
                                $"{currentDirectoryInfo.Parent?.Parent?.FullName ?? currentDirectoryInfo.FullName}" +
                                $"{OperationMaskDefinitions.StartArgumentMarker}";
                            break;
                        }
                }
            }
        }
    }
}