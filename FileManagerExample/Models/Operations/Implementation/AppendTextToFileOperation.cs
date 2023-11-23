namespace FileManagerExample.Models.Operations;

public sealed class AppendTextToFileOperation : Operation
{
    public AppendTextToFileOperation() : base (
        type: OperationTypes.AppendTextToFile,
        command: new OperationCommand(
            declarations: "echo"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                required: true),
            new OperationParameter(
                required: true),
        },
        modifiers: new List<OperationModifier>
        {
            new OperationModifier(
                assignment: OperationModifierAssignments.NewLine,
                required: false,
                declaration: "-l")
        },
        mask: "c m p p")
    {
    }
}