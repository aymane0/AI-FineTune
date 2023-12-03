using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;

public struct Message
{
    public string Role { get; set; }
    public string Content { get; set; }
}
public struct Conversation
{
    public List<Message> Messages { get; set; }
}
public struct RingInfo
{
    public string Image { get; set; }
    public string Name { get; set; }
    public string Effect { get; set; }
    public string Location { get; set; }
}

public class LowerCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
            return name;

        return name.ToLower();
    }
}
public class Program
{
    private static async Task Main()
    {
        var rings = await ScrapeRingsAsync("https://remnant2.wiki.fextralife.com/Rings");
        await SerializeToJsonAsync(rings, "rings.json");

        List<string> lines = new();
        foreach (var ring in rings.Take(50))
        {
            var text = GetRingConversation(ring);
            Console.WriteLine(text);
            lines.Add(text);
        }
        File.WriteAllLines("rings-finetune.jsonl", lines);
    }

    private static string Sanitize(string text)
    {
        return text.Replace(" ", " ").Replace("&nbsp;", " ").Replace("\n", " ").Trim();
    }
    private static async Task<List<RingInfo>> ScrapeRingsAsync(string url)
    {
        var httpClient = new HttpClient();
        var html = await httpClient.GetStringAsync(url);
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var ringsTable = htmlDoc.DocumentNode.SelectSingleNode("//table[@data-key='rings']");
        var rows = ringsTable.SelectNodes(".//tbody/tr");

        return rows.Select(row =>
        {
            var imgNode = row.SelectSingleNode(".//img");
            var image = "https://remnant2.wiki.fextralife.com" + imgNode.GetAttributeValue("src", "").Replace("//", "/");
            var name = imgNode.GetAttributeValue("title", string.Empty).Replace("Remnant 2 ", "");
            var effect = HtmlEntity.DeEntitize(row.SelectSingleNode(".//td[2]").InnerText.Trim());
            var location = HtmlEntity.DeEntitize(row.SelectSingleNode(".//td[3]").InnerText.Trim());

            return new RingInfo
            {
                Image = image,
                Name = Sanitize(name),
                Effect = Sanitize(effect),
                Location = Sanitize(location)
            };
        }).ToList();
    }

    private static async Task SerializeToJsonAsync(List<RingInfo> rings, string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(rings, options);
        await System.IO.File.WriteAllTextAsync(filePath, json);
    }

    public static string GetRingConversation(RingInfo info)
    {
        var conversation = new Conversation
        {
            Messages = new List<Message>
        {
            new Message
            {
                Role = "system",
                Content = "You are a helpful assistant for a Discord server about the game Remnant 2. Give a short 1 line answer on what the requested item does in this format: The `[item name]` ... ."
            },
            new Message
            {
                Role = "user",
                Content = $"What does the ring '{info.Name}' do?"
            },
            new Message
            {
                Role = "assistant",
                Content = $"{info.Effect}"
            },
            new Message
            {
                Role = "user",
                Content = "Where can I find it?"
            },
            new Message
            {
                Role = "assistant",
                Content = $"{info.Location}"
            }
        }
        };
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new LowerCaseNamingPolicy(),
            WriteIndented = false
        };
        string jsonString = JsonSerializer.Serialize(conversation, options);
        return jsonString;
    }
}