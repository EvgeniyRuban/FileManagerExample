namespace FileManagerExample.Models.Operations;

public class OperationModifierInfo : IOperationComponentInfo
{
    private bool _required;

    public OperationModifierInfo(OperationModifierAssignments assignment, bool required)
    {
        Assignment = assignment;
        _required = required;
    }

    public OperationModifierAssignments Assignment { get; set; }
    public OperationComponents ComponentType => OperationComponents.Modifier;
    public bool Required => _required;
}