namespace FileManagerExample.Models.Operations;

public class OperationConstArg
{
    public OperationConstArg(string declaration, OperationConstantArgTypes type)
    {
        Declaration = declaration;
        Type = type;
    }

    public string Declaration { get; }
    public OperationConstantArgTypes Type { get; }
}