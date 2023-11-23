namespace FileManagerExample.Models.Operations;

public sealed class HelpOperation : Operation
{
    public HelpOperation() : base (
        type: OperationTypes.Help,
        command: new OperationCommand(
            declarations: "help"),
        parameters: null!,
        modifiers: null!,
        mask: "c")
    {
    }
}