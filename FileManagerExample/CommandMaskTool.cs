using FileManagerExample.Models.Commands;

namespace FileManagerExample;

public static class CommandMaskTool
{
    public static IList<CommandMaskComponent> ConvertToMaskComponentList(string mask)
    {
        var maskArray = mask.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var maskComponents = new List<CommandMaskComponent>(maskArray.Length);
        foreach (var maskComponent in maskArray)
        {
            switch (maskComponent)
            {
                case CommandMaskDefinitions.Command:
                    maskComponents.Add(CommandMaskComponent.Command);
                    break;
                case CommandMaskDefinitions.Parameter:
                    maskComponents.Add(CommandMaskComponent.Parameter);
                    break;
                case CommandMaskDefinitions.Modifier:
                    maskComponents.Add(CommandMaskComponent.Modifier);
                    break;
                default:
                    maskComponents.Add(CommandMaskComponent.None);
                    break;
            }
        }

        return maskComponents;
    }
}