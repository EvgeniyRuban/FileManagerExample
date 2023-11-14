using FileManagerExample;
using FileManagerExample.Models.Configurations;

var config = Configuration.GetConfiguration();
var startupSettings = await config.GetAsync<StartupSettings>();

if (startupSettings.UseDirectoryFromLastSession)
{
    Console.Title = startupSettings.LastDirectoryPath;
}

Console.ReadKey();