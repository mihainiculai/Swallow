using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.City;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers;

[Authorize]
[Route("api/discover")]
[ApiController]
public class DiscoverController(ICityRepository cityRepository, IMapper mapper) : ControllerBase
{
    [HttpGet("top-cities")]
    public async Task<IActionResult> GetTopCities()
    {
        var cities = await cityRepository.TopCitiesAsync(5);
        return Ok(mapper.Map<IEnumerable<CityDiscoverDto>>(cities));
    }
}