namespace FileManagerExample.Models.FileManager
{
    public struct FileManagerErrorDescriptions
    {
        public const string PathIsUndefined = "The received parameter cannot be identified as a path.";
        public const string NotEmptyDirectoryRemove = "Cannot remove not empty directory.";
        public const string DestinationFileExists = "Destination file is already exists.";
        public const string SourceAndDestComponentSame = "Source and destination components cannot navigate to the same directory component.";
        public const string FileCannotIncludeDirectory = "Attempt to copy directory to the file is impossible.";
        public const string UsingDirectoryInOperationWithFile = "This operation does not accept a directory in place of a file.";
        public const string CreateFileOperationToCreateDirectory = "You cannot use an operation to create a file as an operation to create a directory.";
        public const string ComponentWithCurrentNameExists = "Current name taken!";
    }
}