using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Country;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Implementations;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers
{
    [Authorize]
    [Route("api/countries")]
    [ApiController]
    public class CountryController(IReadOnlyRepository<Country, short> countryRepository, IMapper mapper) : ControllerBase
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

            if (country == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CountryDto>(country));
        }

        [HttpGet("{id}/cities")]
        public async Task<ActionResult<IEnumerable<CountryCityDto>>> GetCitiesByCountryId(short id)
        {
            var country = await countryRepository.GetByIdAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<CountryCityDto>>(country.Cities));
        }
    }
}
