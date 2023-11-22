namespace FileManagerExample.Models.Operations;

public sealed class ShowFileContentOperation : Operation
{
    public ShowFileContentOperation() : base (
        type: OperationTypes.ShowFileContent,
        command: new OperationCommand(
            declarations: "cat"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                required: true),
        },
        modifiers: null!,
        mask: "c p")
    {
        
    }
}