namespace FileManagerExample.Models.Operations;

public sealed class MakeDirectoryOperation : Operation
{
    public MakeDirectoryOperation() : base(
        type: OperationTypes.MakeDirectory,
        command: new OperationCommand(
            declarations: "mkdir"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                type: OperationParameterTypes.FreeForm,
                required: true)
        },
        modifiers: null!,
        mask: "c p")
    {
    }
}