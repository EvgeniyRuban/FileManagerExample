namespace FileManagerExample.Models.Operations;

public enum OperationTypes
{
    ChangeCurrentDirectory,
    MakeDirectory,
    ListDirectoryContent,
    RenameFileOrDirectoryTitle,
    RemoveFileOrDirectory,
    CreateMultipleFiles,
    ShowFileContent,
    AppendTextToFile,
    CopyFileOrDirectory,
    None
}