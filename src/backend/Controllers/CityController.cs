using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.City;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers
{
    [Route("api/cities")]
    [ApiController]
    public class CityController(IReadOnlyRepository<City, int> cityRepository, IMapper mapper) : ControllerBase
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
    }
}
