namespace FileManagerExample.Models.Operations;

public class OperationCommand : IOperationMaskComponent
{
    private bool _required;

    public OperationCommand(OperationType commandType, bool required, params string[] designations)
    {
        Type = commandType;
        _required = required;
        Designations = designations;
    }

    public IList<string> Designations { get; init; }
    public OperationType Type { get; init; }
    public OperationMaskComponent ComponentType => OperationMaskComponent.Command;
    public bool Required => _required;
}