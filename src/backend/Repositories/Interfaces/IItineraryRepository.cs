using Swallow.DTOs.Itinerary;
using Swallow.Models;

namespace Swallow.Repositories.Interfaces;

public interface IItineraryRepository
{
    Task ClearItineraryAsync(Trip trip);
    Task<Trip> CreateItineraryAsync(User user, CreateItineraryDto dto);
    Task<Trip> GetByIdAsync(Guid guid);
    Task AddAttractionAsync(Trip trip, int attractionId);
    Task ReorderAttractionAsync(Trip trip, ReorderAttractionDto dto);
    Task DeleteAttractionAsync(Trip trip, DeleteAttractionDto dto);
    Task UpdateLodgingAsync(Trip trip, string placeId, string sessionToken);
}