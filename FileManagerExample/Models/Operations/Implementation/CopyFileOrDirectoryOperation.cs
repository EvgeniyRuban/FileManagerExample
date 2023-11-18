namespace FileManagerExample.Models.Operations;

public sealed class CopyFileOrDirectoryOperation : Operation
{
    public CopyFileOrDirectoryOperation() : base(
        type: OperationTypes.CopyFileOrDirectory,
        command: new OperationCommand(
            declarations: "cp"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                type: OperationParameterTypes.Path,
                required: true),
            new OperationParameter(
                type: OperationParameterTypes.Path,
                required: true)
        },
        modifiers: null!,
        mask: "c p p")
    {
    }
}