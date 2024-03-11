using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Attraction;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Implementations;
using Swallow.Repositories.Interfaces;
using Swallow.Utils;
using Swallow.Utils.AttractionDataProviders;
using System.Collections.Generic;

namespace Swallow.Controllers
{
    [Route("api/attractions")]
    [ApiController]
    public class AttractionController(AttractionRepository attractionRepository, AttractionCategoryRepository attractionCategoryRepository, CurrencyRepository currencyRepository, IRepository<City, int> cityRepository, TripAdvisorAttractionsCollector tripAdvisorAttractionsCollector, GoogleMapsAttractionsDataFetcher googleMapsAttractionsDataFetcher, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAttractionsDto>>> GetAttractions([FromQuery] int? cityId)
        {
            IEnumerable<GetAttractionsDto> result;

            if (cityId is not null)
            {
                City? city = await cityRepository.GetByIdAsync((int)cityId);

                if (city == null) return NotFound("City not found");

                result = mapper.Map<IEnumerable<GetAttractionsDto>>(await attractionRepository.GetByCityIdAsync((int)cityId));
            }
            else
            {
                result = mapper.Map<IEnumerable<GetAttractionsDto>>(await attractionRepository.GetAllAsync());
            }

            return Ok(result);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<AttractionCategory>>> GetCategories()
        {
            return Ok(mapper.Map<IEnumerable<GetAttractionCategoryDto>>(await attractionCategoryRepository.GetAllAsync()));
        }

        [HttpPost("trip-advisor")]
        public async Task<ActionResult> CollectTripAdvisorAttractions([FromBody] PostTripAdvisorDto postTripAdvisorDto)
        {
            City? city = await cityRepository.GetByIdAsync(postTripAdvisorDto.CityId);
            if (city == null) return NotFound("City not found");
            if (city.TripAdvisorUrl == null) return BadRequest("City TripAdvisor URL not found");

            Currency? currency = await currencyRepository.GetByCodeAsync("USD");
            if (currency == null) return StatusCode(500);

            List<TripAdvisorAttraction> attractions = await tripAdvisorAttractionsCollector.GetAttractionsAsync(city.TripAdvisorUrl);

            await attractionRepository.CreateOrUpdateAsync(attractions, city, currency);

            return Ok();
        }

        [HttpPost("google-maps")]
        public async Task<ActionResult> CollectGoogleMapsAttractionDetails([FromBody] PostGoogleMapsDto postGoogleMapsDto)
        {
            City? city = await cityRepository.GetByIdAsync(postGoogleMapsDto.CityId);
            if (city == null) return NotFound("City not found");

            if (postGoogleMapsDto.AttractionId != null)
            {
                Attraction? attraction = await attractionRepository.GetByIdAsync((int)postGoogleMapsDto.AttractionId);

                if (attraction == null) return NotFound("Attraction not found");

                attraction = await googleMapsAttractionsDataFetcher.AddAttractionDetails(attraction);

                if (attraction == null) return NotFound("Attraction not found on Google Maps");

                return Ok("Attraction details updated successfully");
            }
            else
            {
                IEnumerable<Attraction> attractions = await attractionRepository.GetByCityIdAsync(postGoogleMapsDto.CityId);

                await googleMapsAttractionsDataFetcher.AddAttractionsDetails(attractions);

                return Ok("All attractions details founded on Google Maps updated successfully");
            }
        }

        [HttpPost("clear-city/{cityId}")]
        public async Task<ActionResult> ClearCity(int cityId)
        {
            City? city = await cityRepository.GetByIdAsync(cityId);
            if (city == null) return NotFound("City not found");

            await attractionRepository.ClearCityAsync(city);

            return Ok("City attractions cleared successfully");
        }

        [HttpPost("sync")]
        public async Task<ActionResult> SyncCity([FromQuery] int cityId)
        {
            City? city = await cityRepository.GetByIdAsync(cityId);
            if (city == null) return NotFound("City not found");
            if (city.TripAdvisorUrl == null) return BadRequest("City TripAdvisor URL not found");

            Currency? currency = await currencyRepository.GetByCodeAsync("USD");
            if (currency == null) return StatusCode(500);

            List<TripAdvisorAttraction> attractions = await tripAdvisorAttractionsCollector.GetAttractionsAsync(city.TripAdvisorUrl);

            await attractionRepository.CreateOrUpdateAsync(attractions, city, currency);

            IEnumerable<Attraction> attractionsToSync = await attractionRepository.GetByCityIdAsync(cityId);

            await googleMapsAttractionsDataFetcher.AddAttractionsDetails(attractionsToSync);

            return Ok("City attractions synced successfully");
        }
    }
}
