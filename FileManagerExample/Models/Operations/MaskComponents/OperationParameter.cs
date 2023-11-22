namespace FileManagerExample.Models.Operations;

public class OperationParameter : IOperationMaskComponent
{
    private bool _required;

    public OperationParameter(bool required)
    {
        _required = required;
    }

    public OperationComponents ComponentType => OperationComponents.Parameter;
    public bool Required => _required;
}