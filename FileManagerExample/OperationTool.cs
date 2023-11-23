using FileManagerExample.Models.Operations;

namespace FileManagerExample;

public static class OperationTool
{
    public static IList<IOperationMaskComponent> ConvertToMaskComponentList(string mask,
                                                                            OperationCommand command,
                                                                            List<OperationParameter> parameters,
                                                                            List<OperationModifier> modifiers)
    {
        var maskArray = mask.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var maskComponents = new List<IOperationMaskComponent>(maskArray.Length);
        int currentParameterIndex = 0;
        int currentModifierIndex = 0;

        foreach (var maskComponent in maskArray)
        {
            switch (maskComponent)
            {
                case OperationMaskDefinitions.Command:
                    maskComponents.Add(command);
                    break;
                case OperationMaskDefinitions.Parameter:
                    maskComponents.Add(parameters[currentParameterIndex++]);
                    break;
                case OperationMaskDefinitions.Modifier:
                    maskComponents.Add(modifiers[currentModifierIndex++]);
                    break;
                default:
                    maskComponents.Add(null!);
                    break;
            }
        }

        return maskComponents;
    }
}