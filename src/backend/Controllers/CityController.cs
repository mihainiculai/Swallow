using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using Swallow.DTOs.City;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;
using Swallow.Utils;

namespace Swallow.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CityController(IReadOnlyRepository<City, int> cityRepository, IMapper mapper, IConfiguration configuration, TripAdvisorAttractionsCollector attractionUtils) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetCityById(int id)
        {
            var city = await cityRepository.GetByIdAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CityDto>(city));
        }

        [HttpGet("generate-description/{id}")]
        public async Task<ActionResult<string>> GenerateDescription(int id)
        {
            var city = await cityRepository.GetByIdAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok("Description of " + city.Name + ", " + city.Country + ".");

            OpenAIAPI api = new OpenAIAPI(configuration["OpenAI:AdminKey"]!);

            ChatRequest chatRequest = new()
            {
                Model = new Model("gpt-4-0125-preview") { OwnedBy = "openai" },
                Temperature = 0.0,
                MaxTokens = 500,
                ResponseFormat = ChatRequest.ResponseFormats.Text,
                Messages = [
                    new(ChatMessageRole.System, "You are a helpful assistant for a trip website. You must return a one paragraph description of the city."),
                    new(ChatMessageRole.User, "Make a description of the city of " + city.Name + ", " + city.Country + ".")
                ]
            };

            ChatResult results = await api.Chat.CreateChatCompletionAsync(chatRequest);

            return Ok(results.Choices[0].Message.TextContent);
        }

        //test
        [HttpGet("attractions")]
        public async Task<OkObjectResult> GetAttractions()
        {
            var attractions = await attractionUtils.GetAttractionsAsync("https://www.tripadvisor.com/Attractions-g298478-Activities-oa0-Timisoara_Timis_County_Western_Romania_Transylvania.html");
            return Ok(attractions);
        }
    }
}
