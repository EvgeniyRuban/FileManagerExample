using FileManagerExample;
using FileManagerExample.Models.Configurations;
using FileManagerExample.Models.Operations;

#region Initialization

string userName = Environment.UserName;
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
    UpdateConsoleTitle(currentDirectoryPath);
    Console.Write($"{userName}: ");
    var input = Console.ReadLine();
    var operationInfo = OperationAnalizer.GetOperationAnalysis(input, currentDirectoryPath);

    if (!operationInfo.Success)
    {
        var answer = operationInfo.ErrorInfo;
        if (operationInfo.OperationDetected)
        {
            answer += $"\n{OperationExamples.Examples[operationInfo.OperationType]}";
        }

        Console.WriteLine(answer);
        continue;
    }

    var fileManagerInfo = FileManager.ExecuteOperation(operationInfo, ref currentDirectoryPath);

    if (!fileManagerInfo.Succes)
    {
        Console.WriteLine(fileManagerInfo.Error);
    }
}
while(true);

void UpdateConsoleTitle(string title) => Console.Title = title;