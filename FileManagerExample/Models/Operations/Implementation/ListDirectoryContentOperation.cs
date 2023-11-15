namespace FileManagerExample.Models.Operations;

public class ListDirectoryContentOperation : Operation
{
    public ListDirectoryContentOperation() : base(
        command: new OperationCommand(
            commandType: OperationType.ListDirectoryContent,
            required: true,
            designations: "ls"),
        parameters: null!,
        modifiers: new List<OperationModifier>
        {
            new OperationModifier(
                required: false,
                designation: "-a")
        },
        mask: "c m")
    {
    }
}