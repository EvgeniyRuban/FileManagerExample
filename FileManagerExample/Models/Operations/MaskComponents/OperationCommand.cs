namespace FileManagerExample.Models.Operations;

public class OperationCommand : IOperationMaskComponent
{
    public OperationCommand(params string[] declarations)
    {
        Declarations = declarations;
    }

    public IList<string> Declarations { get; init; }
    public OperationComponents ComponentType => OperationComponents.Command;
    public bool Required => true;
}