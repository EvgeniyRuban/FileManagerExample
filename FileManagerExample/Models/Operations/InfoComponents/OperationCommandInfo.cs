namespace FileManagerExample.Models.Operations;

public class OperationCommandInfo : IOperationComponentInfo
{
    public OperationComponents ComponentType => OperationComponents.Command;
    public bool Required => true;
}