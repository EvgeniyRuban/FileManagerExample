namespace FileManagerExample.Models.Operations;

public sealed class CopyFileOrDirectoryOperation : Operation
{
    public CopyFileOrDirectoryOperation() : base(
        type: OperationTypes.CopyFileOrDirectory,
        command: new OperationCommand(
            declarations: "cp"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                required: true),
            new OperationParameter(
                required: false)
        },
        modifiers: new List<OperationModifier>
        {
            new OperationModifier(
                assignment: OperationModifierAssignments.Overwrite,
                required: false,
                declaration: "-o"),
            new OperationModifier(
                assignment: OperationModifierAssignments.Recursive,
                required: false,
                declaration: "-r")
        },
        mask: "c m m p p")
    {
    }
}