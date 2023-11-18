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
                type: OperationParameterTypes.FreeForm,
                required: true),
            new OperationParameter(
                type: OperationParameterTypes.FreeForm,
                required: false),
            new OperationParameter(
                type: OperationParameterTypes.FreeForm,
                required: false),
            new OperationParameter(
                type: OperationParameterTypes.FreeForm,
                required: false),
            new OperationParameter(
                type: OperationParameterTypes.FreeForm,
                required: false),
        },
        modifiers: null!,
        mask: "c p p p p p")
    { 
    }
}