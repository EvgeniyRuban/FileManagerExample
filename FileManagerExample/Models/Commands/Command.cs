namespace FileManagerExample.Models.Commands;

public abstract class Command
{
    public Command(CommandType commandType, string mask, params string[] designations)
    {
        Designations = designations;
        CommandType = commandType;
        MaskComponents = CommandMaskTool.ConvertToMaskComponentList(mask);
    }

    public IList<string> Designations { get; set; }
    public CommandType CommandType { get; set; }
    public IList<CommandMaskComponent> MaskComponents { get; private set; }
    public IList<CommandModifier>? Modifiers { get; protected set; }
}
