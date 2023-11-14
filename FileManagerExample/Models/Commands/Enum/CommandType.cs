namespace FileManagerExample.Models.Commands;

public enum CommandType
{
    ChangeCurrentDirectory,
    CreateDirectory,
    ShowDirectoryContent,
    RenameDirectoryTitle,
    DeleteDirectory,
    CreateFile,
    RenameFile,
    FillFile,
    AppendFile,
    ClearFile,
    DeleteFile,
    OpenFile,
}
