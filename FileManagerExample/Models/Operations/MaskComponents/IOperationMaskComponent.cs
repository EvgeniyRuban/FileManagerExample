namespace FileManagerExample.Models.Operations;

public interface IOperationMaskComponent
{
    OperationMaskComponent ComponentType { get; }
    bool Required { get; }
}