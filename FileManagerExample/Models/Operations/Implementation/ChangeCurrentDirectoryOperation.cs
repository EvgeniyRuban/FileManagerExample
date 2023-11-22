namespace FileManagerExample.Models.Operations;

public sealed class ChangeCurrentDirectoryOperation : Operation
{
    public ChangeCurrentDirectoryOperation() : base(
        type: OperationTypes.ChangeCurrentDirectory,
        command: new OperationCommand(
            declarations: "cd"),
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