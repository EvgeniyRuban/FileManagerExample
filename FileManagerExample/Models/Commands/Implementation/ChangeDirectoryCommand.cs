namespace FileManagerExample.Models.Commands;

public class ChangeDirectoryCommand : Command
{
    public ChangeDirectoryCommand() : base(commandType: CommandType.ChangeCurrentDirectory,
                                           mask: "c p",
                                           designations: new string[] {"cd", "changedirectory"})
    {
    }
}