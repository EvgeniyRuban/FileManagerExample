using FileManagerExample;
using FileManagerExample.Models.Configurations;

#region Initialization

string currentDirectory = string.Empty;
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
    var input = Console.ReadLine();
    var operationAnalysis = OperationAnalizer.GetOperationAnalysis(input, currentDirectoryPath);

    #region Print

    Console.WriteLine($"Operation detected: {operationAnalysis.OperationDetected}");
    Console.WriteLine($"Operation type: {operationAnalysis.OperationType}");

    Console.WriteLine($"Parameters count: {operationAnalysis.Parameters.Count}");
    for (int i = 0; i < operationAnalysis.Parameters.Count; i++)
    {
        Console.WriteLine($"Parameter-{i + 1}:\n\tType: {operationAnalysis.Parameters[i].Type}\n\tValue: {operationAnalysis.Parameters[i].Value}");
    }

    Console.WriteLine($"Modifiers count: {operationAnalysis.Modifiers.Count}");
    for (int i = 0; i < operationAnalysis.Modifiers.Count; i++)
    {
        Console.WriteLine($"Modifier-{i + 1}:\n\tAssignment: {operationAnalysis.Modifiers[i].Assignment}");
    }

    Console.WriteLine($"Succes: {operationAnalysis.Success}");
    Console.WriteLine($"Error info: {operationAnalysis.ErrorInfo}");

    #endregion

    Console.ReadKey();
    Console.Clear();
}
while(true) ;