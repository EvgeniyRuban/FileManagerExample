namespace FileManagerExample.Models.Operations;

public class OperationModifier : IOperationMaskComponent
{
    public readonly string? Declaration;
    private readonly bool _required;

    public OperationModifier(OperationModifierAssignments assignment, bool required, string declaration)
    {
        Assignment = assignment;
        _required = required;
        Declaration = declaration;
    }

    public OperationModifierAssignments Assignment { get; }
    public OperationComponents ComponentType => OperationComponents.Modifier;
    public bool Required => _required;
}