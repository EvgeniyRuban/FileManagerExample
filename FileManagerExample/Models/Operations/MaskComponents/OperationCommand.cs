namespace FileManagerExample.Models.Operations;

public class OperationCommand : IOperationMaskComponent
{
    public OperationCommand(params string[] designations)
    {
        Designations = designations;
    }

    public IList<string> Designations { get; init; }
    public OperationComponents ComponentType => OperationComponents.Command;
    public bool Required => true;
}