using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.City;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController(ICityRepository cityRepository, IMapper mapper) : ControllerBase
    {
        private const int threshold = 60;
        
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
    }
}
