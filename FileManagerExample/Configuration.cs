using FileManagerExample.Models.Configurations;
using Newtonsoft.Json;

namespace FileManagerExample;

public class Configuration
{
    private static Configuration _config;

    private Configuration() { }
    static Configuration()
    {
        _config = new Configuration();
    }

    public static Configuration GetConfiguration() => _config;

    /// <summary>
    /// Get configurations from <typeparamref name="TConfigurationObject"/> file.
    /// </summary>
    public TConfigurationObject? Get<TConfigurationObject>() where TConfigurationObject : IConfigurationObject
    {
        var path = GetPathByObjectType(typeof(TConfigurationObject));
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<TConfigurationObject>(json);
    }

    /// <summary>
    /// Get configurations from <typeparamref name="TConfigurationObject"/> file asynchronously.
    /// </summary>
    public async Task<TConfigurationObject?> GetAsync<TConfigurationObject>() where TConfigurationObject : IConfigurationObject
    {
        var path = GetPathByObjectType(typeof(TConfigurationObject));
        var json = await File.ReadAllTextAsync(path);
        return JsonConvert.DeserializeObject<TConfigurationObject>(json);
    }

    private string GetPathByObjectType(Type type) => $"{Environment.CurrentDirectory}/{type.Name}.json";
}