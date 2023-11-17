namespace FileManagerExample.Models.Operations;

public abstract class Operation
{
    public Operation(
        OperationTypes type,
        OperationCommand command, 
        List<OperationParameter> parameters, 
        List<OperationModifier> modifiers, 
        string mask)
    {
        Type = type;
        Command = command;
        Parameters = parameters ?? new List<OperationParameter>(); ; 
        Modifiers = modifiers ?? new List<OperationModifier>();
        MaskComponents = OperationMaskTool.ConvertToMaskComponentList(mask, command, parameters, modifiers);
    }

    public OperationTypes Type { get; set; }
    public OperationCommand Command { get; }
    public IList<IOperationMaskComponent> MaskComponents { get; }
    public IList<OperationParameter> Parameters { get; protected set; }
    public IList<OperationModifier> Modifiers { get; protected set; }

    public (int MinIndex, int MaxIndex) GetCommandPositionRangeInMask()
    {
        var maxIndex = MaskComponents.IndexOf(Command);
        var minIndex = MaskComponents.Take(maxIndex).Count(c => c.Required);
        return (minIndex, maxIndex);
    }
}