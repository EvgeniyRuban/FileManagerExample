namespace FileManagerExample.Models.Operations;

public class ChangeCurrentDirectoryOperation : Operation
{
    public ChangeCurrentDirectoryOperation() : base(
        type: OperationTypes.ChangeCurrentDirectory,
        command: new OperationCommand(
            designations: "cd"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                type: OperationParameterTypes.Path,
                required: true)
        },
        modifiers: null!,
        mask: "c p")
    {
    }
}