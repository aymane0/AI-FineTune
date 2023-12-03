using System.IO;
using Newtonsoft.Json.Linq;

namespace gpt_remnant;

public static class OpenAIConfig
{
    public static string GetApiKey()
    {
        string secretsPath = Path.Combine(Directory.GetCurrentDirectory(), "secrets.json");
        string secretsContent = File.ReadAllText(secretsPath);
        var secretsJson = JObject.Parse(secretsContent);
        return secretsJson.GetValue("OpenAIApiKey")!.ToString();
    }
}