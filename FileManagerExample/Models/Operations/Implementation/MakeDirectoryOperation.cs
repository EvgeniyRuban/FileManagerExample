namespace FileManagerExample.Models.Operations;

public class MakeDirectoryOperation : Operation
{
    public MakeDirectoryOperation() : base(
        command: new OperationCommand(
            commandType: OperationType.MakeDirectory,
            required: true,
            designations: "mkdir"),
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