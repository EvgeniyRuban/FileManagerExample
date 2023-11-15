namespace FileManagerExample.Models.Operations;

public class ChangeCurrentDirectoryOperation : Operation
{
    public ChangeCurrentDirectoryOperation() : base(
        command: new OperationCommand(
            commandType: OperationType.ChangeCurrentDirectory,
            required: true,
            designations: "cd"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                required: true)
        },
        modifiers: null!,
        mask: "c p")
    {
    }
}