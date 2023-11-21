namespace FileManagerExample.Models.FileManager;

public class FileManagerInfo
{
    public FileManagerInfo()
    {
    }
    public FileManagerInfo(bool succes, string error = null!)
    {
        Succes = succes;
        Error = error;
    }

    public bool Succes { get; set; }
    public string? Error { get; set; }
}