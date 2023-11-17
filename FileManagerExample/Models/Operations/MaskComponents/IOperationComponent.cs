namespace FileManagerExample.Models.Operations;

public interface IOperationComponent
{
    OperationComponents ComponentType { get; }
    bool Required { get; }
}
