namespace FileManagerExample.Models.Operations;

public sealed class RenameDirectoryTitleOperation : Operation
{
    public RenameDirectoryTitleOperation() : base(
        type: OperationTypes.RenameDirectoryTitle,
        command: new OperationCommand(
            designations: "mv"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                type: OperationParameterTypes.FreeForm,
                required: true),
            new OperationParameter(
                type: OperationParameterTypes.FreeForm,
                required: true)
        },
        modifiers: null!,
        mask: "c p p")
    {
    }
}