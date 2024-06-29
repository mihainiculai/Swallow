using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.City;
using Swallow.DTOs.Search;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using Swallow.Utils.GoogleMaps;

namespace Swallow.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController(IGoogleMapsSearch googleMapsSearch, ICityRepository cityRepository, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Search(string? query)
        {
            IEnumerable<City> cities;
            
            if (query is null)
            {
                cities = await cityRepository.TopCitiesAsync(5);
            }
            else
            {
                cities = cityRepository.Search(query);
            }
            
            return Ok(mapper.Map<IEnumerable<CitySearchDto>>(cities));
        }
        
        [HttpGet("places")]
        public async Task<IActionResult> SearchHotels([FromQuery] int cityId, [FromQuery] string query, [FromQuery] Guid? sessionToken)
        {
            var city = await cityRepository.GetByIdAsync(cityId);
            sessionToken ??= Guid.NewGuid();

            var predictions = await googleMapsSearch.AutocompleteSearchPlaceAsync(city, query, (Guid)sessionToken);
            
            PlaceSearchDto placeSearchDto = new()
            {
                Predictions = predictions,
                SessionToken = (Guid)sessionToken
            };

            return Ok(placeSearchDto);
        }
    }
}
