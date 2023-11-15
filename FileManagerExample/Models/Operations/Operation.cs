namespace FileManagerExample.Models.Operations;

public abstract class Operation
{
    public Operation(OperationCommand command, List<OperationParameter> parameters, List<OperationModifier> modifiers, string mask)
    {
        Command = command;
        Parameters = parameters;
        Modifiers = modifiers;
        MaskComponents = OperationMaskTool.ConvertToMaskComponentList(mask, command, parameters, modifiers);
    }

    public OperationCommand Command { get; }
    public IList<IOperationMaskComponent> MaskComponents { get; }
    public IList<OperationParameter> Parameters { get; protected set; }
    public IList<OperationModifier>? Modifiers { get; protected set; }
}