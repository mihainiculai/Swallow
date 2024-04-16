using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.City;
using Swallow.Repositories.Interfaces;
using Swallow.Utils.OpenAi;

namespace Swallow.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/cities")]
    [ApiController]
    public class CityController(ICityRepository cityRepository, ILocationDescriptionGenerator locationDescriptionGenerator, IMapper mapper) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetCityById(int id)
        {
            var city = await cityRepository.GetByIdAsync(id);

            return Ok(mapper.Map<CityDto>(city));
        }

        [HttpGet("{id}/generate-description")]
        public async Task<ActionResult<string>> GenerateDescription(int id)
        {
            var city = await cityRepository.GetByIdAsync(id);

            var description = await locationDescriptionGenerator.GenerateCityDescriptionAsync(city.Name, city.Country.Name);

            return Ok(description);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCity([FromBody] CityDto cityDto)
        {
            await cityRepository.UpdateAsync(cityDto);

            return Ok();
        }
    }
}
