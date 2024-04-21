using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Preference;
using Swallow.Repositories.Interfaces;
using Swallow.Services.Recommendation;

namespace Swallow.Controllers;

public class ItineraryController(IUserRepository userRepository, IAttractionRecommender attractionRecomandar, IMapper mapper) : ControllerBase
{
    [HttpGet("recomand-attractions")]
    public async Task<IActionResult> RecomandAttractions([FromQuery] int cityId)
    {
        var user = await userRepository.GetUserAsync(User);
        
        var recommendations = await attractionRecomandar.GetRecommendations(user, cityId);
        
        return Ok(mapper.Map<List<AttractionRecomandation>>(recommendations));
    }
}