namespace FileManagerExample.Models.Operations;

public class OperationArgInfo : IOperationComponentInfo
{
    private readonly bool _required;

    public OperationArgInfo(bool required = false)
    {
        _required = required;
    }

    public string? Value { get; set; }
    public OperationComponents ComponentType => OperationComponents.Parameter;
    public bool Required => _required;
}