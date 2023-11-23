using FileManagerExample;
using FileManagerExample.Models.Configurations;

#region Initialization

var currentDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
var config = Configuration.GetConfiguration();
var startupSettings = await config.GetAsync<StartupSettings>();

if (startupSettings.UseDirectoryFromLastSession)
{
    currentDirectoryPath = startupSettings.LastDirectoryPath;
}

#endregion

do
{
    Console.Title = currentDirectoryPath;
    ConsoleOutputTool.PrintOperationLineStart();
    var input = Console.ReadLine();
    var operationInfo = OperationAnalizer.GetOperationAnalysis(input, currentDirectoryPath);

    if (!operationInfo.Success)
    {
        ConsoleOutputTool.PrintErrorInfo(operationInfo.ErrorInfo);
        if (operationInfo.OperationDetected)
        {
            ConsoleOutputTool.PrintOperationExample(operationInfo);
        }

        continue;
    }

    var fileManagerInfo = FileManager.ExecuteOperation(operationInfo, ref currentDirectoryPath);

    if (!fileManagerInfo.Succes)
    {
        ConsoleOutputTool.PrintErrorInfo(fileManagerInfo.ErrorInfo);
    }
}
while(true);