using FileManagerExample.Models.FileManager;
using FileManagerExample.Models.Operations;

namespace FileManagerExample;

public static class FileManager
{
    public static FileManagerInfo ExecuteOperation(OperationInfo operationInfo, ref string currentDirectory)
    {
        switch (operationInfo.OperationType)
        {
            case OperationTypes.ListDirectoryContent: return ListDirectoryContent(currentDirectory);
            case OperationTypes.ChangeCurrentDirectory: return ChangeCurrentDirectory(operationInfo, ref currentDirectory);
            case OperationTypes.MakeDirectory: return MakeDirectory(operationInfo, currentDirectory);
            case OperationTypes.RenameFileOrDirectoryTitle: return RenameFileOrDirectoryTitle(operationInfo, currentDirectory);
            case OperationTypes.RemoveFileOrDirectory: return RemoveFileOrDirectory(operationInfo, currentDirectory);
            case OperationTypes.CreateMultipleFiles: return CreateMultipleFiles(operationInfo, currentDirectory);
            case OperationTypes.ShowFileContent: return ShowFileContent(operationInfo, currentDirectory);
            case OperationTypes.AppendTextToFile: return AppendTextToFile(operationInfo, currentDirectory);
            case OperationTypes.CopyFileOrDirectory: return CopyFileOrDirectory(operationInfo, currentDirectory);
            case OperationTypes.ClearConsole: return ClearConsole();
            default: return new FileManagerInfo(true);
        }
    }

    private static FileManagerInfo ListDirectoryContent(string currentDirectory)
    {
        var directoryComponents = Directory.EnumerateFileSystemEntries(currentDirectory);

        foreach (string component in directoryComponents)
        {
            string text = string.Empty;
            bool isFile = File.Exists(component);

            if (isFile)
            {
                text = Path.GetFileName(component);
            }
            else
            { var directory = new DirectoryInfo(component);
                text = $"{directory.Name}{Path.DirectorySeparatorChar}";
            }

            Console.WriteLine(text);
        }

        return new FileManagerInfo(true);
    }

    private static FileManagerInfo ChangeCurrentDirectory(OperationInfo operationInfo, ref string currentDirectory)
    {
        var destDirectoryPath = operationInfo.Args[0].Value;

        if (!Directory.Exists(destDirectoryPath))
        {
            destDirectoryPath = $"{currentDirectory}{Path.DirectorySeparatorChar}{destDirectoryPath}";

            if (!Directory.Exists(destDirectoryPath))
            {
                return new FileManagerInfo
                {
                    Succes = false,
                    Error = FileManagerErrorDescriptions.PathIsUndefined
                };
            }
        }

        currentDirectory = destDirectoryPath;

        return new FileManagerInfo(true);
    }

    private static FileManagerInfo MakeDirectory(OperationInfo operationInfo, string currentDirectory)
    {
        var newDirectoryTitle = operationInfo.Args[0].Value;
        var newDirectoryPath = $"{currentDirectory}{Path.DirectorySeparatorChar}{newDirectoryTitle}";
        Directory.CreateDirectory(newDirectoryPath);

        return new FileManagerInfo(true);
    }

    private static FileManagerInfo RemoveFileOrDirectory(OperationInfo operationInfo, string currentDirectory)
    {
        var componentPath = $"{currentDirectory}{Path.DirectorySeparatorChar}{operationInfo.Args[0].Value}";

        if (Directory.Exists(componentPath))
        {
            var isDirectoryEmpty = Directory.EnumerateFileSystemEntries(componentPath).Count() == 0;

            if (!isDirectoryEmpty)
            {
                var acceptRecursiveRemoveModifier = operationInfo.Modifiers
                    .FirstOrDefault(m => m.Assignment == OperationModifierAssignments.Recursive);

                if (acceptRecursiveRemoveModifier is null)
                {
                    return new FileManagerInfo
                    {
                        Succes = false,
                        Error = FileManagerErrorDescriptions.NotEmptyDirectoryRemove
                    };
                }
            }
            Directory.Delete(componentPath, true);
            return new FileManagerInfo(true);
        }
        else if ( File.Exists(componentPath))
        {
            File.Delete(componentPath);
            return new FileManagerInfo(true);
        }

        return new FileManagerInfo
        {
            Succes = false,
            Error = FileManagerErrorDescriptions.PathIsUndefined
        };
    }

    private static FileManagerInfo RenameFileOrDirectoryTitle(OperationInfo operationInfo, string currentDirectory)
    {
        var sourceComponent = ConvertToFileSystemInfo(operationInfo.Args[0].Value, currentDirectory);

        if (sourceComponent == null)
        {
            return new FileManagerInfo
            {
                Succes = false,
                Error = FileManagerErrorDescriptions.PathIsUndefined
            };
        }

        var sourceComponentParentPath = Directory.GetParent(sourceComponent.FullName).FullName;
        var newComponentPath = Path.Combine(sourceComponentParentPath, operationInfo.Args[1].Value);

        if (sourceComponent is FileInfo)
        {
            if(File.Exists(newComponentPath))
            {
                return new FileManagerInfo
                {
                    Succes = false,
                    Error = FileManagerErrorDescriptions.ComponentWithCurrentNameExists
                };
            }

            var sourceFile = sourceComponent as FileInfo;
            sourceFile.CopyTo(newComponentPath);
            sourceFile.Delete();
        }
        else
        {
            if (Directory.Exists(newComponentPath))
            {
                return new FileManagerInfo
                {
                    Succes = false,
                    Error = FileManagerErrorDescriptions.ComponentWithCurrentNameExists
                };
            }

            Directory.CreateDirectory(newComponentPath);
            CopyDirectory(sourceComponent.FullName, newComponentPath, true);
            Directory.Delete(sourceComponent.FullName, true);
        }

        return new FileManagerInfo(true);
    }

    private static FileManagerInfo CreateMultipleFiles(OperationInfo operationInfo, string currentDirectory)
    {
        foreach(var parameter in operationInfo.Args)
        {
            var currentFilePath = Path.Combine(currentDirectory, parameter.Value);
            var file = new FileInfo(currentFilePath);

            if (!file.Directory.Exists)
            {
                return new FileManagerInfo
                {
                    Succes = false,
                    Error = FileManagerErrorDescriptions.CreateFileOperationToCreateDirectory
                };
            }

            using var stream = File.Create(currentFilePath);
        }

        return new FileManagerInfo(true);
    }

    private static FileManagerInfo ShowFileContent(OperationInfo operationInfo, string currentDirectory)
    {
        var componentToPrint = ConvertToFileSystemInfo(operationInfo.Args[0].Value, currentDirectory);
        if (componentToPrint == null)
        {
            return new FileManagerInfo
            {
                Succes = false,
                Error = FileManagerErrorDescriptions.PathIsUndefined
            };
        }

        if (componentToPrint is not FileInfo) 
        {
            return new FileManagerInfo
            {
                Succes = false,
                Error = FileManagerErrorDescriptions.UsingDirectoryInOperationWithFile
            };
        }
        string text = File.ReadAllText(componentToPrint.FullName);
        Console.WriteLine(text);
        return new FileManagerInfo(true);
    }

    private static FileManagerInfo AppendTextToFile(OperationInfo operationInfo, string currentDirectory)
    {
        var filePath = $"{currentDirectory}{Path.DirectorySeparatorChar}{operationInfo.Args[0].Value}";
        File.AppendAllText(filePath, operationInfo.Args[1].Value);
        return new FileManagerInfo(true);
    }

    private static FileManagerInfo CopyFileOrDirectory(OperationInfo operationInfo, string currentDirectory)
    {
        var sourceComponent = ConvertToFileSystemInfo(operationInfo.Args[0].Value, currentDirectory);
        var destComponentPath = operationInfo.Args.Count() == 2 
            ? operationInfo.Args[1].Value 
            : currentDirectory;
        var destComponent = ConvertToFileSystemInfo(destComponentPath, currentDirectory);

        if (sourceComponent == null || destComponent == null)
        {
            return new FileManagerInfo
            {
                Succes = false,
                Error = FileManagerErrorDescriptions.PathIsUndefined
            };
        }

        if (sourceComponent.FullName.ToLower() == destComponent.FullName.ToLower())
        {
            return new FileManagerInfo
            {
                Succes = false,
                Error = FileManagerErrorDescriptions.SourceAndDestComponentSame
            };
        }

        var isRecursive = operationInfo.Modifiers.Any(m => m.Assignment == OperationModifierAssignments.Recursive);
        var useOverwrite = operationInfo.Modifiers.Any(m => m.Assignment == OperationModifierAssignments.Overwrite);

        if (sourceComponent is FileInfo && destComponent is FileInfo)
        {
            if (!useOverwrite)
            {
                return new FileManagerInfo
                {
                    Succes = false,
                    Error = FileManagerErrorDescriptions.DestinationFileExists
                };
            }

            File.Copy(sourceComponent.FullName, destComponent.FullName, true);
        }
        else if (sourceComponent is FileInfo && destComponent is DirectoryInfo)
        {
            string fileCopyPath;
            if (destComponent.FullName.ToLower() == currentDirectory.ToLower())
            {
                fileCopyPath = GetAvaliableFileCopyPath(sourceComponent as FileInfo);
            }
            else
            {
                fileCopyPath = $"{destComponent.FullName}{Path.DirectorySeparatorChar}{sourceComponent.Name}";
            }
            File.Copy(sourceComponent.FullName, fileCopyPath);
        }
        else if (sourceComponent is DirectoryInfo && destComponent is FileInfo) 
        {
            return new FileManagerInfo
            {
                Succes = false,
                Error = FileManagerErrorDescriptions.FileCannotIncludeDirectory
            };
        }
        else if (sourceComponent is DirectoryInfo && destComponent is DirectoryInfo)
        {
            CopyDirectory(sourceComponent.FullName, destComponent.FullName, isRecursive);
        }

        return new FileManagerInfo(true);
    }

    private static FileManagerInfo ClearConsole()
    {
        Console.Clear();
        return new FileManagerInfo(true);
    }

    private static FileSystemInfo? ConvertToFileSystemInfo(string componentPath, string parentDirectory)
    {
        var path = Path.Combine(parentDirectory, componentPath);
        var file = new FileInfo(path);

        if (file.Exists)
        {
            return file;
        }

        var directory = new DirectoryInfo(path);

        if (directory.Exists)
        {
            return directory;
        }

        return null;
    }

    private static string GetAvaliableFileCopyPath(FileInfo file)
    {
        int iterator = 0;
        string avaliableFileCopyPath;
        do
        {
            iterator++;
            avaliableFileCopyPath = $"{file.Directory.FullName}{Path.DirectorySeparatorChar}({iterator}){file.Name}";
        }
        while (File.Exists(avaliableFileCopyPath));

        return avaliableFileCopyPath;
    }

    private static void CopyDirectory(string sourceDirectoryPath, string destDirectoryPath, bool recursive)
    {
        var sourceDirectory = new DirectoryInfo(sourceDirectoryPath);
        var directories = sourceDirectory.GetDirectories();
        Directory.CreateDirectory(destDirectoryPath);

        foreach (var file in sourceDirectory.GetFiles()) 
        {
            var targetFilePath = Path.Combine(destDirectoryPath, file.Name);
            file.CopyTo(targetFilePath);
        }

        if (recursive)
        {
            foreach (DirectoryInfo subDirectory in directories)
            {
                string newDestinationDir = Path.Combine(destDirectoryPath, subDirectory.Name);
                CopyDirectory(subDirectory.FullName, newDestinationDir, true);
            }
        }
    }
}