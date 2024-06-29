using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.Repositories.Implementations;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers;

[Authorize]
[Route("api/trips")]
[ApiController]
public class TripController(ITransportRepository transportRepository) : ControllerBase
{
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