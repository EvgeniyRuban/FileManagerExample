namespace FileManagerExample.Models.Operations;

public sealed class ClearConsoleOperation : Operation
{
    public ClearConsoleOperation() : base(
        type: OperationTypes.ClearConsole,
        command: new OperationCommand(
            declarations: "clear"),
        parameters: null!,
        modifiers: null!,
        mask: "c")
    {
    }
}