namespace FileManagerExample.Models.Operations;

public sealed class ListDirectoryContentOperation : Operation
{
    public ListDirectoryContentOperation() : base(
        type: OperationTypes.ListDirectoryContent,
        command: new OperationCommand(
            declarations: "ls"),
        parameters: null!,
        modifiers: null!,
        mask: "c")
    {
    }
}