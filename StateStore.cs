using System.IO;
using System.Text.Json;

namespace DvrkTweaks.Services;

public static class StateStore
{
    private static string P => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "DVRK",
        "state.json");

    public static Dictionary<string, bool> Load()
    {
        try
        {
            return File.Exists(P)
                ? JsonSerializer.Deserialize<Dictionary<string, bool>>(File.ReadAllText(P)) ?? new()
                : new();
        }
        catch
        {
            return new();
        }
    }

    public static void Save(Dictionary<string, bool> state)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(P)!);
        File.WriteAllText(P, JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true }));
    }
}
