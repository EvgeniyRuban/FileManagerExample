namespace FileManagerExample.Models.Operations;

public sealed class CreateMultipleFilesOperation : Operation
{
    public CreateMultipleFilesOperation() : base(
        type: OperationTypes.CreateMultipleFiles,
        command: new OperationCommand(
            declarations: "touch"),
        parameters: new List<OperationParameter>
        {
            new OperationParameter(
                required: true),
            new OperationParameter(
                required: false),
            new OperationParameter(
                required: false),
            new OperationParameter(
                required: false),
            new OperationParameter(
                required: false),
        },
        modifiers: null!,
        mask: "c p p p p p")
    { 
    }
}