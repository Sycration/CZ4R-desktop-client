using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Org.OpenAPITools.Api;

namespace App.Services;

public class AuthPersistenceService
{
    private static readonly string Path = System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "cz4r-client", "auth.json");

    public static AuthData Load()
    {
        try
        {
            if (File.Exists(Path))
                return JsonSerializer.Deserialize<AuthData>(File.ReadAllText(Path)) ?? new();
        }
        catch { }
        return new();
    }

    public static void Save(string serverUrl, string token)
    {
        try
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path)!);
            File.WriteAllText(Path, JsonSerializer.Serialize(new AuthData { ServerUrl = serverUrl, Token = token }));
        }
        catch { }
    }

    public static void Clear()
    {
        try { File.Delete(Path); } catch { }
    }
}

public class AuthData
{
    public string ServerUrl { get; set; } = "";
    public string Token { get; set; } = "";
}
