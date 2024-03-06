using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using Swallow.Data;
using Swallow.DTOs.Attraction;
using Swallow.DTOs.City;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Implementations;
using Swallow.Repositories.Interfaces;
using Swallow.Utils;
using Swallow.Utils.AttractionDataProviders;

namespace Swallow.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CityController(ApplicationDbContext context, IRepository<City, int> cityRepository, CurrencyRepository currencyRepository, AttractionRepository attractionRepository, GoogleMapsAttractionsDataFetcher googleMapsAttractionsDataFetcher, IMapper mapper, IConfiguration configuration, TripAdvisorAttractionsCollector attractionUtils) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetCityById(int id)
        {
            City? city = await cityRepository.GetByIdAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CityDto>(city));
        }

        [HttpGet("{id}/generate-description")]
        public async Task<ActionResult<string>> GenerateDescription(int id)
        {
            City? city = await cityRepository.GetByIdAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            OpenAIAPI api = new(configuration["OpenAI:AdminKey"]!);

            ChatRequest chatRequest = new()
            {
                Model = new Model("gpt-4-0125-preview") { OwnedBy = "openai" },
                Temperature = 0.0,
                MaxTokens = 500,
                ResponseFormat = ChatRequest.ResponseFormats.Text,
                Messages = [
                    new(ChatMessageRole.System, "You are a helpful assistant for a trip website. You must return a 75 words one paragraph description of the city."),
                    new(ChatMessageRole.User, "Make a description of the city of " + city.Name + ", " + city.Country + ".")
                ]
            };

            ChatResult results = await api.Chat.CreateChatCompletionAsync(chatRequest);

            return Ok(results.Choices[0].Message.TextContent);
        }

        [HttpPost("{id}/attractions")]
        public async Task<ActionResult> GetAttractions(int id, [FromBody] PostTripAdvisorDto postAttractionsDto)
        {
            City? city = await cityRepository.GetByIdAsync(id);
            if (city == null) return NotFound();

            Currency? currency = await currencyRepository.GetByCodeAsync("USD");
            if (currency == null) return StatusCode(500);

            List<TripAdvisorAttraction> attractions = await attractionUtils.GetAttractionsAsync(postAttractionsDto.TripAdvisorUrl);

            await attractionRepository.CreateOrUpdateAsync(attractions, city, currency);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCity(int id, [FromBody] CityDto cityDto)
        {
            City? city = await cityRepository.GetByIdAsync(id);
            if (city == null) return NotFound();

            city = mapper.Map(cityDto, city);

            await cityRepository.UpdateAsync(city);

            return Ok();
        }
    }
}
