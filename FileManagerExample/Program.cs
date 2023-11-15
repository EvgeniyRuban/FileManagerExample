using FileManagerExample;
using FileManagerExample.Models.Configurations;

#region Initialization

string currentDirectory = string.Empty;
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
    var operation = OperationAnalizer.GetOperation(input);

    if(operation != null)
    {
        #region Print

        Console.WriteLine($"CommandType: {operation.Command.Type}");
        Console.Write("Designations: ");
        foreach(var item in operation.Command.Designations)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();

        if(operation.Modifiers != null)
        {
            Console.Write("Modifiers: ");
            foreach (var item in operation.Modifiers)
            {
                Console.Write($"{item.Designation}[Required:{item.Required}] ");
            }
            Console.WriteLine();
        }

        Console.Write("MaskComponents: ");
        foreach (var item in operation.MaskComponents)
        {
            Console.Write($"{item.ComponentType} ");
        }
        Console.WriteLine();
        #endregion
    }

    Console.ReadKey();
    Console.Clear();
}
while(true) ;