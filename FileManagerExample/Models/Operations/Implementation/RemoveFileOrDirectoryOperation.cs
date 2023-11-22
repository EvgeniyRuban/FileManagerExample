namespace FileManagerExample.Models.Operations;

public sealed class RemoveFileOrDirectoryOperation : Operation
{
    public RemoveFileOrDirectoryOperation() : base(
        type: OperationTypes.RemoveFileOrDirectory,
        command: new OperationCommand(
            declarations: "rm"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                required: true),
        },
        modifiers: new List<OperationModifier>
        {
            new OperationModifier(
                assignment: OperationModifierAssignments.Recursive,
                required: false,
                declaration: "-r")
        },
        mask: "c m p")
    {
    }
}