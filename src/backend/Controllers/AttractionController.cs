using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Attraction;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Implementations;
using Swallow.Repositories.Interfaces;
using Swallow.Utils;
using Swallow.Utils.AttractionDataProviders;

namespace Swallow.Controllers
{
    [Route("api/attractions")]
    [ApiController]
    public class AttractionController(AttractionRepository attractionRepository, CurrencyRepository currencyRepository, IReadOnlyRepository<City, int> cityRepository, TripAdvisorAttractionsCollector tripAdvisorAttractionsCollector, GoogleMapsAttractionsDataFetcher googleMapsAttractionsDataFetcher) : ControllerBase
    {
        [HttpPost("trip-advisor")]
        public async Task<ActionResult> CollectTripAdvisorAttractions([FromBody] PostTripAdvisorDto postTripAdvisorDto)
        {
            City? city = await cityRepository.GetByIdAsync(postTripAdvisorDto.CityId);
            if (city == null) return NotFound("City not found");

            Currency? currency = await currencyRepository.GetByCodeAsync("USD");
            if (currency == null) return StatusCode(500);

            List<TripAdvisorAttraction> attractions = await tripAdvisorAttractionsCollector.GetAttractionsAsync(postTripAdvisorDto.TripAdvisorUrl);

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
    }
}
