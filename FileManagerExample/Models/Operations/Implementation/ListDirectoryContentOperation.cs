namespace FileManagerExample.Models.Operations;

public class ListDirectoryContentOperation : Operation
{
    public ListDirectoryContentOperation() : base(
        type: OperationTypes.ListDirectoryContent,
        command: new OperationCommand(
            designations: "ls"),
        parameters: null!,
        modifiers: new List<OperationModifier>()
        {
            new OperationModifier(
                assignment: OperationModifierAssignments.ListIncludeHiddenFiles,
                required: false,
                designation: "-a")
        },
        mask: "c m")
    {
    }
}