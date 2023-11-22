namespace FileManagerExample.Models.Operations;

public sealed class RenameFileOrDirectoryTitleOperation : Operation
{
    public RenameFileOrDirectoryTitleOperation() : base(
        type: OperationTypes.RenameFileOrDirectoryTitle,
        command: new OperationCommand(
            declarations: "mv"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                required: true),
            new OperationParameter(
                required: true)
        },
        modifiers: null!,
        mask: "c p p")
    {
    }
}