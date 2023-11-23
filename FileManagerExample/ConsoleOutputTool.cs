using FileManagerExample.Models.Operations;

namespace FileManagerExample;

public static class ConsoleOutputTool
{
    private static string _operationLineStart;

    static ConsoleOutputTool()
    {
        _operationLineStart = $"{Environment.UserName}: ";
    }

    public static void Print(string text, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black) 
    {
        Console.ForegroundColor = foreground;
        Console.BackgroundColor = background;
        Console.Write(text);
        Console.ResetColor();
    }
    public static void PrintLine(string text, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
    {
        Console.ForegroundColor = foreground;
        Console.BackgroundColor = background;
        Console.WriteLine(text);
        Console.ResetColor();
    }
    public static void PrintOperationLineStart() => Print(_operationLineStart, foreground: ConsoleColor.Yellow);
    public static void PrintErrorInfo(string errorInfo) => PrintLine(errorInfo, foreground: ConsoleColor.Red);
    public static void PrintOperationExample(OperationInfo operationInfo) => PrintLine(OperationExamples.Examples[operationInfo.OperationType]);
    public static void PrintAllOperationExamples()
    {
        foreach(var key in OperationExamples.Examples.Keys)
        {
            PrintLine(OperationExamples.Examples[key]);
        }
    }
    public static void PrintDirectoryContent(List<FileSystemInfo> fileSystemInfo)
    {
        var directoryColor = ConsoleColor.Blue;
        var fileColor = ConsoleColor.Cyan;

        foreach (var item in fileSystemInfo )
        {
            PrintLine(item.Name, foreground: item is FileInfo ? fileColor : directoryColor);
        }
    }
}