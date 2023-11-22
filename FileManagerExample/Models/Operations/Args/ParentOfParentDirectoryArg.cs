namespace FileManagerExample.Models.Operations;

public sealed class ParentOfParentDirectoryArg : OperationConstArg
{
    public ParentOfParentDirectoryArg() : base ("../..", OperationConstantArgTypes.ParantOfParentDirectory)
    {
    }
}