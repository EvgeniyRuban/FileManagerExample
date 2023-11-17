namespace FileManagerExample.Models.Operations;

public enum OperationTypes
{
    ChangeCurrentDirectory, // done
    MakeDirectory, // done
    ListDirectoryContent, // done
    RenameDirectoryTitle,
    DeleteDirectory,
    CreateFile,
    RenameFile,
    FillFile,
    AppendFile,
    ClearFile,
    DeleteFile,
    OpenFile,
    None
}