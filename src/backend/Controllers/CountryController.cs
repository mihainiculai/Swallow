using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Country;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountryController(IReadOnlyRepository<Country, int> countryRepository, IMapper mapper) : ControllerBase
    {
        private readonly IReadOnlyRepository<Country, int> _countryRepository = countryRepository;
        private readonly IMapper _mapper = mapper; 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllCountries()
        {
            var countries = await _countryRepository.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<CountryDto>>(countries));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountryById(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CountryDto>(country));
        }
    }
}
