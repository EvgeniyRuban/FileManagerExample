namespace FileManagerExample.Models.Operations;

public class OperationParameter : IOperationMaskComponent
{
    private bool _required;

    public OperationParameter(bool required)
    {
        _required = required;
    }

    public OperationMaskComponent ComponentType => OperationMaskComponent.Parameter;
    public bool Required => _required;
}