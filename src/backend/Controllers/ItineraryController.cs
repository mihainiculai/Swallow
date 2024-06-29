using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Itinerary;
using Swallow.DTOs.Preference;
using Swallow.Repositories.Interfaces;
using Swallow.Services.Recommendation;

namespace Swallow.Controllers;

[Authorize]
[Route("api/itineraries")]
[ApiController]
public class ItineraryController(IItineraryRepository itineraryRepository, IUserRepository userRepository, IAttractionRecommender attractionRecomandar, IMapper mapper) : ControllerBase
{
    [HttpGet("{tripId}")]
    public async Task<IActionResult> GetItinerary(Guid tripId)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();

        return Ok(mapper.Map<ItineraryDto>(trip));
    }
    
    [HttpGet("recommend-attractions/{tripGuid}")]
    public async Task<IActionResult> RecommendAttractions(Guid tripGuid)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripGuid);
        var recommendations = await attractionRecomandar.GetRecommendations(user, trip.CityId, 5, trip);
        
        return Ok(mapper.Map<List<AttractionRecomandation>>(recommendations));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateItinerary([FromBody] CreateItineraryDto dto)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.CreateItineraryAsync(user, dto);
        
        return Ok(trip.TripId);
    }
}