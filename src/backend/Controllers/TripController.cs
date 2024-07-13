using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Trip;
using Swallow.Repositories.Implementations;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers;

[Authorize]
[Route("api/trips")]
[ApiController]
public class TripController(ITransportRepository transportRepository, IUserRepository userRepository, IMapper mapper) : ControllerBase
{
    [Route("upcoming")]
    [HttpGet]
    public async Task<IActionResult> GetUpcomingTrips()
    {
        var user = await userRepository.GetUserAsync(User);
        var trips = await transportRepository.GetUpcomingTripsAsync(user);
        
        return Ok(mapper.Map<IEnumerable<TripDto>>(trips));
    }
    
    [Route("transport-modes")]
    [HttpGet]
    public async Task<IActionResult> GetTransportModes()
    {
        var transportModes = await transportRepository.GetAllTransportModesAsync();
        return Ok(transportModes);
    }
    
    [Route("transport-types")]
    [HttpGet]
    public async Task<IActionResult> GetTransportTypes()
    {
        var transportTypes = await transportRepository.GetAllTransportTypesAsync();
        return Ok(transportTypes);
    }
}