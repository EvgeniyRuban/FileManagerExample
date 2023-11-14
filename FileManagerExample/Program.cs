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
    var command = CommandAnalizer.Analize(input);

    if(command != null)
    {
        Console.WriteLine($"CommandType: {command.CommandType}");
        Console.Write("Designations: ");
        foreach(var item in command.Designations)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();

        if(command.Modifiers != null)
        {
            Console.Write("Modifiers: ");
            foreach (var item in command.Modifiers)
            {
                Console.Write($"{item.Designation}[Required:{item.Required}] ");
            }
            Console.WriteLine();
        }

        Console.Write("MaskComponents: ");
        foreach (var item in command.MaskComponents)
        {
            Console.Write($"{item} ");
        }
        Console.WriteLine();
    }

    Console.ReadKey();
    Console.Clear();
}
while(true) ;