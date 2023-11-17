namespace FileManagerExample.Models.Operations;

public class MakeDirectoryOperation : Operation
{
    public MakeDirectoryOperation() : base(
        type: OperationTypes.MakeDirectory,
        command: new OperationCommand(
            designations: "mkdir"),
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