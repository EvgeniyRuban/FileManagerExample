namespace FileManagerExample.Models.Operations;

public sealed class AppendTextToFileOperation : Operation
{
    public AppendTextToFileOperation() : base (
        type: OperationTypes.AppendTextToFile,
        command: new OperationCommand(
            declarations: "echo"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                type: OperationParameterTypes.FreeForm,
                required: true),
        },
        modifiers: null!,
        mask: "c p")
    {
        
    }
}