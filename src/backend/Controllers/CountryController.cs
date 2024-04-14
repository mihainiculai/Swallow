using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Country;
using Swallow.Repositories.Interfaces;
using Swallow.Utils.OpenAi;

namespace Swallow.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/countries")]
    [ApiController]
    public class CountryController(ICountryRepository countryRepository, ILocationDescriptionGenerator locationDescriptionGenerator, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllCountries()
        {
            var countries = await countryRepository.GetAllAsync();
            return Ok(mapper.Map<IEnumerable<CountryDto>>(countries));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountryById(short id)
        {
            var country = await countryRepository.GetByIdAsync(id);
            return Ok(mapper.Map<CountryDetailsDto>(country));
        }

        [HttpGet("{id}/generate-description")]
        public async Task<ActionResult<string>> GenerateDescription(short id)
        {
            var country = await countryRepository.GetByIdAsync(id);
            string description = await locationDescriptionGenerator.GenerateCountryDescriptionAsync(country.Name);
            return Ok(description);
        }

        [HttpGet("{id}/cities")]
        public async Task<ActionResult<IEnumerable<CountryCityDto>>> GetCitiesByCountryId(short id)
        {
            var country = await countryRepository.GetByIdAsync(id);
            return Ok(mapper.Map<IEnumerable<CountryCityDto>>(country.Cities));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCountry([FromBody] CountryDetailsDto countryDto)
        {
            await countryRepository.UpdateAsync(countryDto);
            return Ok();
        }
    }
}
