namespace FileManagerExample.Models.Operations;

public class OperationInfo
{
    public OperationTypes OperationType { get; set; } = OperationTypes.None;
    public IList<OperationParameterInfo> Parameters { get; set; } = new List<OperationParameterInfo>();
    public IList<OperationModifierInfo> Modifiers { get; set; } = new List<OperationModifierInfo>();
    public string? ErrorInfo { get; set; }
    public bool Success { get; set; }
    public bool OperationDetected { get; set; }
}