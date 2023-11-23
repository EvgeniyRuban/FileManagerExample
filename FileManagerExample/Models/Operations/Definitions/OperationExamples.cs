namespace FileManagerExample.Models.Operations;

public static class OperationExamples
{
    public static readonly Dictionary<OperationTypes, string> Examples = new ()
    {
        { OperationTypes.ChangeCurrentDirectory, "cd \"dest_dir\" - to change current directory" },
        { OperationTypes.MakeDirectory, "mkdir \"dir_name\" - to create new directory" },
        { OperationTypes.ListDirectoryContent, "ls - list directory content" },
        { OperationTypes.RenameFileOrDirectoryTitle, "mv \"src_file_name\" \"new_file_name\" - to rename file" +
            "\nmv \"src_dir_name\" \"new_dir_name\" - to rename directory" },
        { OperationTypes.RemoveFileOrDirectory, "rm \"file\" - to remove file\nrm -r \"dir\" - to remove not empty directory" },
        { OperationTypes.CreateMultipleFiles, "touch \"file_1\" ... \"file_name_5\" - to create multiple files, but not less than 1" },
        { OperationTypes.ShowFileContent, "cat \"file\" - to show file content" },
        { OperationTypes.AppendTextToFile, "echo \"dest_file\" \"text_to_append\" - append text to file" },
        { OperationTypes.CopyFileOrDirectory, "cp \"src_file\" \"dest_file\" - to copy file" +
            "\ncp \"src_dir\" \"dest_dir\" - to copy directory" +
            "\ncp -r \"src_dir\" \"dest_dir\" - to copy directory hierarchy" },
        { OperationTypes.ClearConsole, "clear - to clear console" },
    };
}