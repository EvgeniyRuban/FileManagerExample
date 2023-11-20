using FileManagerExample;
using FileManagerExample.Models.Configurations;
using FileManagerExample.Models.Operations;

#region Initialization

string currentDirectory = string.Empty;
string userName = Environment.UserName;
var currentDirectoryPath = @"C:\Users\eugen\OneDrive\Рабочий стол"; // Временно, для тестирования.
var config = Configuration.GetConfiguration();
var startupSettings = await config.GetAsync<StartupSettings>();

if (startupSettings.UseDirectoryFromLastSession)
{
    currentDirectory = startupSettings.LastDirectoryPath;
}
else
{
    currentDirectory = Directory.GetDirectoryRoot(Environment.CurrentDirectory);
}

Console.Title = currentDirectory;

#endregion

do
{
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

    Print(operationInfo);
}
while(true);

void Print(OperationInfo operationInfo)
{
    Console.WriteLine($"Operation detected: {operationInfo.OperationDetected}");
    Console.WriteLine($"Operation type: {operationInfo.OperationType}");

    Console.WriteLine($"Parameters count: {operationInfo.Parameters.Count}");
    for (int i = 0; i < operationInfo.Parameters.Count; i++)
    {
        Console.WriteLine($"Parameter-{i + 1}:\n\tType: {operationInfo.Parameters[i].Type}\n\tValue: {operationInfo.Parameters[i].Value}");
    }

    Console.WriteLine($"Modifiers count: {operationInfo.Modifiers.Count}");
    for (int i = 0; i < operationInfo.Modifiers.Count; i++)
    {
        Console.WriteLine($"Modifier-{i + 1}:\n\tAssignment: {operationInfo.Modifiers[i].Assignment}");
    }

    Console.WriteLine($"Succes: {operationInfo.Success}");
    Console.WriteLine($"Error info: {operationInfo.ErrorInfo}");
}