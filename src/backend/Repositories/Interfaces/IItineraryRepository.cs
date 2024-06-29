using Swallow.DTOs.Itinerary;
using Swallow.Models;

namespace Swallow.Repositories.Interfaces;

public interface IItineraryRepository
{
    Task ClearItineraryAsync(Trip trip);
    Task<Trip> CreateItineraryAsync(User user, CreateItineraryDto dto);
    Task<Trip> GetByIdAsync(Guid guid);
}