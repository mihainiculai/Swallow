using OpenAI_API.Chat;
using OpenAI_API;
using OpenAI_API.Models;

namespace Swallow.Utils.OpenAi
{
    public interface ILocationDescriptionGenerator
    {
        Task<string> GenerateCityDescriptionAsync(string name, string country);
        Task<string> GenerateCountryDescriptionAsync(string name);
    }

    public class LocationDescriptionGenerator(IConfiguration configuration) : ILocationDescriptionGenerator
    {
        private readonly string OpenAiAdminKey = configuration["OpenAI:AdminKey"]!;
        private readonly Model model = new("gpt-4-0125-preview") { OwnedBy = "openai" };

        private async Task<string> GetDescriptionAsync(string prompt)
        {
            OpenAIAPI api = new(OpenAiAdminKey);

            ChatRequest chatRequest = new()
            {
                Model = model,
                Temperature = 0.0,
                MaxTokens = 500,
                ResponseFormat = ChatRequest.ResponseFormats.Text,
                Messages = [
                    new(ChatMessageRole.System, "You are a helpful assistant for a trip website. You must return a 75 words one paragraph description of the city or country."),
                    new(ChatMessageRole.User, prompt)
                ]
            };

            var results = await api.Chat.CreateChatCompletionAsync(chatRequest);

            return results.Choices[0].Message.TextContent;
        }

        public async Task<string> GenerateCountryDescriptionAsync(string name)
        {
            return await GetDescriptionAsync("Make a description of the country of " + name + ".");
        }

        public async Task<string> GenerateCityDescriptionAsync(string name, string country)
        {
            return await GetDescriptionAsync("Make a description of the city of " + name + ", " + country + ".");
        }
    }
}
