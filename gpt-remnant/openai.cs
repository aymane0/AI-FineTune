using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Chat;
using OpenAI_API.Models;

namespace gpt_remnant;

public class OpenAIClient
{
    OpenAIAPI api;
    public OpenAIClient(string apiKey)
    {
        api = new OpenAIAPI(apiKey);
    }
    public async Task<string> GetAnswerForItem(string systemPrompt, string userPrompt, string model = "gpt-3.5-turbo-1106")
    {
        var chatRequest = new ChatRequest()
        {
            Model = model,
            Temperature = 0.1,
            MaxTokens = 100,
            Messages = new ChatMessage[] {
                new ChatMessage(ChatMessageRole.System, systemPrompt),
                new ChatMessage(ChatMessageRole.User, userPrompt)
            }
        };
        await api.Chat.CreateChatCompletionAsync(chatRequest);

        var chatResponse = await api.Chat.CreateChatCompletionAsync(chatRequest);

        return chatResponse.Choices[0].ToString().Trim();
    }
}