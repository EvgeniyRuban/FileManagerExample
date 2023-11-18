namespace FileManagerExample.Models.Operations;

public class OperationParameter : IOperationMaskComponent
{
    private bool _required;

    public OperationParameter(OperationParameterTypes type, bool required)
    {
        Type = type;
        _required = required;
    }

    public OperationParameterTypes Type { get; }
    public OperationComponents ComponentType => OperationComponents.Parameter;
    public bool Required => _required;
}