namespace FileManagerExample.Models.Operations;

public sealed class CurrentDirectoryArg : OperationConstArg
{
    public CurrentDirectoryArg() : base(".", OperationConstantArgTypes.CurrentDirectory)
    {
    }
}
