namespace FileManagerExample.Models.Operations;

public class OperationModifier : IOperationMaskComponent
{
    public readonly string? Designation;
    private readonly bool _required;

    public OperationModifier(OperationModifierAssignments assignment, bool required, string designation)
    {
        Assignment = assignment;
        _required = required;
        Designation = designation;
    }

    public OperationModifierAssignments Assignment { get; }
    public OperationComponents ComponentType => OperationComponents.Modifier;
    public bool Required => _required;
}