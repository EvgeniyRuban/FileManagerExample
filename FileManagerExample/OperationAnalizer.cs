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
        new CopyFileOrDirectoryOperation()
    };

    public static OperationAnalysisInfo GetOperationAnalysis(string operationText, string currentDirectoryPath)
    {
        var operationAnalysisInfo = new OperationAnalysisInfo();
        var array = operationText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

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
                case OperationComponents.Parameter: operationAnalysisInfo.Parameters.Add((OperationParameterInfo)component); break;
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
                interpretedComponents[i] = new OperationParameterInfo();
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
                    interpretedComponents[j] = new OperationParameterInfo(required: parameter.Required)
                    {
                        Type = parameter.Type,
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
        => text[0] == OperationMaskDefinitions.StartParameterMarker && text[text.Length - 1] == OperationMaskDefinitions.EndParameterMarker;

    private static bool IsModifier(string text, OperationModifier modifier) => modifier.Declaration == text;

    /// <summary>
    /// The method checks for the presence of a parameter by searching for markers of the beginning and end of the parameter; if the parameter is compound, it is joined.
    /// </summary>
    private static void JoinParameters(ref string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            char firstCharacter = array[i][0];
            if (firstCharacter == OperationMaskDefinitions.StartParameterMarker)
            {
                int parameterElementCount = 1;

                for (int j = i; j < array.Length; j++)
                {
                    char lastCharacter = array[j][array[j].Length - 1];

                    if (i == j)
                    {
                        if(lastCharacter == OperationMaskDefinitions.EndParameterMarker && array[j].Length != 1)
                        {
                            break;
                        }
                    }
                    else if (lastCharacter == OperationMaskDefinitions.EndParameterMarker)
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
}