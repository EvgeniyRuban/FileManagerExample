namespace FileManagerExample.Models.Operations;

public sealed class ParentDirectoryArg : OperationConstArg
{
    public ParentDirectoryArg() : base ("..", OperationConstantArgTypes.ParentDirectory)
    {
    }
}
