namespace FileManagerExample.Models.Operations;

public sealed class RemoveFileOrDirectoryOperation : Operation
{
    public RemoveFileOrDirectoryOperation() : base(
        type: OperationTypes.RemoveFileOrDirectory,
        new OperationCommand(
            declarations: "rm"),
        new List<OperationParameter>
        {
            new OperationParameter(
                type: OperationParameterTypes.FreeForm,
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