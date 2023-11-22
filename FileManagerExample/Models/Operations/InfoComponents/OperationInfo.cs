namespace FileManagerExample.Models.Operations;

public class OperationInfo
{
    public OperationTypes OperationType { get; set; } = OperationTypes.None;
    public IList<OperationArgInfo> Args { get; set; } = new List<OperationArgInfo>();
    public IList<OperationModifierInfo> Modifiers { get; set; } = new List<OperationModifierInfo>();
    public string? ErrorInfo { get; set; }
    public bool Success { get; set; }
    public bool OperationDetected { get; set; }
}