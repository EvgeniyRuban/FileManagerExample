namespace FileManagerExample.Models.Operations;

public static class OperationExamples
{
    public static readonly Dictionary<OperationTypes, string> Examples = new ()
    {
        { OperationTypes.ChangeCurrentDirectory, "cd \"destination_directory\" - to change current directory" },
        { OperationTypes.MakeDirectory, "mkdir \"directory_name\" - to create new directory" },
        { OperationTypes.ListDirectoryContent, "ls - list directory content" },
        { OperationTypes.RenameFileOrDirectoryTitle, "mv \"source_file_name\" \"dest_file_name\" - to rename file" +
            "\nmv \"source_directory_name\" \"dest_directory_name\" - to rename directory" },
        { OperationTypes.RemoveFileOrDirectory, "rm \"file_name\" - to remove file\nrm -r \"directory_name\" - to remove directory" },
        { OperationTypes.CreateMultipleFiles, "touch \"file_name_1\" ... \"file_name_5\" - to create multiple files, but not less than 1" },
        { OperationTypes.ShowFileContent, "cat \"file_name\" - to show file content" },
        { OperationTypes.AppendTextToFile, "echo \"dest_file\" \"text_to_append\" - append text to file" },
        { OperationTypes.CopyFileOrDirectory, "cp \"source_file_name\" \"dest_file_name\" - to copy file" +
            "\ncp \"source_directory_name\" \"dest_directory_name\" - to copy directory" },
        { OperationTypes.ClearConsole, "clear - to clear console" },
    };
}