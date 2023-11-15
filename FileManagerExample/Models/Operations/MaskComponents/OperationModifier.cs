namespace FileManagerExample.Models.Operations;

public class OperationModifier : IOperationMaskComponent
{
    public readonly string? Designation;
    private readonly bool _required;

    public OperationModifier(bool required, string designation)
    {
        _required = required;
        Designation = designation;
    }

    public OperationMaskComponent ComponentType => OperationMaskComponent.Modifier;
    public bool Required => _required;
}