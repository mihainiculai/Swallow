using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Destination;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers;

[Route("api/destinations")]
[ApiController]
public class DestinationController(ICityRepository cityRepository, IAttractionRepository attractionRepository, IMapper mapper) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<DestinationDto>> GetDestinationById(int id)
    {
        var city = await cityRepository.GetByIdAsync(id);
        return Ok(mapper.Map<DestinationDto>(city));
    }
    
    [HttpGet("{id}/top-attractions")]
    public async Task<ActionResult<IEnumerable<DestinationAttractionDto>>> GetTopAttractions(int id)
    {
        var attractions = await attractionRepository.GetByCityIdAsync(id, 10);
        return Ok(mapper.Map<IEnumerable<DestinationAttractionDto>>(attractions));
    }
}