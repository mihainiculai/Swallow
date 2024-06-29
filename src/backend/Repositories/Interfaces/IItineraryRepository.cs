using Swallow.DTOs.Itinerary;
using Swallow.Models;

namespace Swallow.Repositories.Implementations;

public interface IItineraryRepository
{
    Task<Trip> CreateManualItinerary(User user, CreateItineraryDto dto);
}