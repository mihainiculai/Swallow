using Swallow.Models;

namespace Swallow.Repositories.Interfaces;

public interface IPlaceRepository
{
    Task<Place> GetByPlaceIdAsync(string googlePlaceId, string? sessionToken = null);
}