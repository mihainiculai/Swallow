using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.DTOs.Itinerary;
using Swallow.DTOs.Preference;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using Swallow.Services.Recommendation;

namespace Swallow.Controllers;

[Authorize]
[Route("api/itineraries")]
[ApiController]
public class ItineraryController(IItineraryRepository itineraryRepository, IAttachmentRepository attachmentRepository, IUserActionRepository userActionRepository, IUserRepository userRepository, IAttractionRepository attractionRepository, IAttractionRecommender attractionRecomandar, IMapper mapper) : ControllerBase
{
    [HttpGet("{tripId}")]
    public async Task<IActionResult> GetItinerary(Guid tripId)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();

        return Ok(mapper.Map<ItineraryDto>(trip));
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchAttractions([FromQuery] Guid tripId, [FromQuery] string query)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        var attractions = await attractionRepository.GetByNameAsync(trip.CityId, query);
        
        return Ok(mapper.Map<List<SearchAttractionResponseDto>>(attractions));
    }
    
    [HttpGet("recommend-attractions/{tripGuid}")]
    public async Task<IActionResult> RecommendAttractions(Guid tripGuid)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripGuid);
        var recommendations = await attractionRecomandar.GetRecommendations(user, trip.CityId, 5, trip);
        
        return Ok(mapper.Map<List<AttractionRecomandation>>(recommendations));
    }
    
    [HttpPost("add-attraction/{tripId}/{attractionId}")]
    public async Task<IActionResult> AddAttraction(Guid tripId, int attractionId)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        await itineraryRepository.AddAttractionAsync(trip, attractionId);
        await userActionRepository.CreateAsync(user.Id, attractionId, 2);
        
        return Ok();
    }
    
    [HttpPost("reorder-attraction/{tripId}")]
    public async Task<IActionResult> ReorderAttraction(Guid tripId, [FromBody] ReorderAttractionDto dto)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        await itineraryRepository.ReorderAttractionAsync(trip, dto);
        
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateItinerary([FromBody] CreateItineraryDto dto)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.CreateItineraryAsync(user, dto);
        
        return Ok(trip.TripId);
    }
    
    [HttpGet("preferences")]
    public async Task<IActionResult> GetPreferences([FromQuery] int attractionId)
    {
        var user = await userRepository.GetUserAsync(User);
        var preference = await userActionRepository.GetUserPreference(user, attractionId);
        
        return Ok(preference);
    }
    
    [HttpPost("preferences")]
    public async Task<IActionResult> SetPreferences([FromQuery] int attractionId, [FromQuery] int preference)
    {
        var user = await userRepository.GetUserAsync(User);
        await userActionRepository.CreateAsync(user.Id, attractionId, preference);
        
        return Ok();
    }
    
    [HttpDelete("{tripId}/attractions")]
    public async Task<IActionResult> DeleteAttractions(Guid tripId, [FromBody] DeleteAttractionDto dto)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        var attractionId = await itineraryRepository.DeleteAttractionAsync(trip, dto);
        await userActionRepository.CreateAsync(user.Id, attractionId, 3);
        
        return Ok();
    }
    
    [HttpPost("{tripId}/lodging")]
    public async Task<IActionResult> UpdateLodging(Guid tripId, [FromBody] UpdateLodgingDto dto)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        await itineraryRepository.UpdateLodgingAsync(trip, dto.PlaceId, dto.SessionToken);
        
        return Ok();
    }
    
    [HttpGet("{tripId}/attachments")]
    public async Task<IActionResult> GetAttachments(Guid tripId)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        var attachments = await attachmentRepository.GetByTripIdAsync(tripId);
        
        return Ok(attachments);
    }
    
    [HttpGet("{tripId}/attachments/{attachmentId}")]
    public async Task<IActionResult> GetAttachment(Guid tripId, int attachmentId)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        var attachment = await attachmentRepository.GetByIdAsync(attachmentId);
        
        return File(attachment.Content, attachment.ContentType, attachment.FileName);
    }
    
    [HttpPost("{tripId}/attachments")]
    public async Task<IActionResult> UploadAttachment(Guid tripId, IFormFile file)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        if (file.Length == 0) return BadRequest("File is empty");
        if (file.Length > 10485760) return BadRequest("File is too large");
        
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var attachment = new Attachment
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = memoryStream.ToArray(),
                UploadDate = DateTime.Now,
                TripId = tripId
            };
            
            await attachmentRepository.CreateAsync(attachment);
        }

        return Ok();
    }
    
    [HttpDelete("{tripId}/attachments/{attachmentId}")]
    public async Task<IActionResult> DeleteAttachment(Guid tripId, int attachmentId)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        await attachmentRepository.DeleteAsync(attachmentId);
        
        return Ok();
    }
    
    [HttpPost("{tripId}/archive")]
    public async Task<IActionResult> ArchiveTrip(Guid tripId)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        await itineraryRepository.ArchiveTripAsync(trip);
        
        return Ok();
    }
    
    [HttpPost("{tripId}/unarchive")]
    public async Task<IActionResult> UnarchiveTrip(Guid tripId)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        await itineraryRepository.UnarchiveTripAsync(trip);
        
        return Ok();
    }
    
    [HttpDelete("{tripId}")]
    public async Task<IActionResult> DeleteTrip(Guid tripId)
    {
        var user = await userRepository.GetUserAsync(User);
        var trip = await itineraryRepository.GetByIdAsync(tripId);
        
        if (trip.UserId != user.Id) return Unauthorized();
        
        await itineraryRepository.DeleteTripAsync(trip);
        
        return Ok();
    }
}