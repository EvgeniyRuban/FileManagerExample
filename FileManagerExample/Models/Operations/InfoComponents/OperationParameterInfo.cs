namespace FileManagerExample.Models.Operations;

public class OperationParameterInfo : IOperationComponentInfo
{
    private readonly bool _required;

    public OperationParameterInfo(bool required = false)
    {
        _required = required;
    }

    public string? Value { get; set; }
    public OperationParameterTypes Type { get; set; }
    public OperationComponents ComponentType => OperationComponents.Parameter;
    public bool Required => _required;
}