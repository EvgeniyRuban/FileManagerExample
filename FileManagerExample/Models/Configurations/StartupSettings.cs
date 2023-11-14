using FileManagerExample.Intarfaces;

namespace FileManagerExample.Models.Configurations;

public class StartupSettings : IConfigurationObject
{
    public bool UseDirectoryFromLastSession { get; set; }
    public string? LastDirectoryPath { get; set; }
}