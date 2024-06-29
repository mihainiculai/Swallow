using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Attraction;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using Swallow.Utils.AttractionDataProviders;

namespace Swallow.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/attractions")]
    [ApiController]
    public class AttractionController(IAttractionRepository attractionRepository, IAttractionCategoryRepository attractionCategoryRepository, ICityRepository cityRepository, ITripAdvisorAttractionsCollector tripAdvisorAttractionsCollector, IGoogleMapsAttractionsDataFetcher googleMapsAttractionsDataFetcher, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAttractionsDto>>> GetAttractions([FromQuery] int? cityId)
        {
            var attractions = await attractionRepository.GetAllAsync(cityId);
            var result = mapper.Map<IEnumerable<GetAttractionsDto>>(attractions);
            return Ok(result);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<AttractionCategory>>> GetCategories()
        {
            IEnumerable<AttractionCategory> result = await attractionCategoryRepository.GetAllAsync();
            return Ok(mapper.Map<IEnumerable<GetAttractionCategoryDto>>(result));
        }

        [HttpPost("sync")]
        public async Task<ActionResult> SyncCity([FromQuery] int cityId)
        {
            var city = await cityRepository.GetByIdAsync(cityId);

            if (city.TripAdvisorUrl == null)
            {
                return BadRequest("City TripAdvisor URL not found");
            }

            var attractions = await tripAdvisorAttractionsCollector.GetAttractionsAsync(city.TripAdvisorUrl);
            await attractionRepository.CreateOrUpdateAsync(attractions, city);
            
            var attractionsToSync = await attractionRepository.GetAllAsync(cityId);
            await googleMapsAttractionsDataFetcher.AddAttractionsDetailsAsync(attractionsToSync);

            await cityRepository.CalculateScoreAsync(city);
            await attractionRepository.NormalizeRatingAsync();

            return Ok("City attractions synced successfully");
        }
        
        [HttpPost("normalize-rating")]
        public async Task<ActionResult> NormalizeRating()
        {
            await attractionRepository.NormalizeRatingAsync();
            return Ok("Attractions rating normalized successfully");
        }

        [HttpPost("clear-city/{cityId}")]
        public async Task<ActionResult> ClearCity(int cityId)
        {
            var city = await cityRepository.GetByIdAsync(cityId);

            await attractionRepository.ClearTrashAsync(city);

            return Ok("City attractions cleared successfully");
        }
    }
}
