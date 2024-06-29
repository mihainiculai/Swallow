using Swallow.Models;

namespace Swallow.Repositories.Implementations;

public interface IPlaceRepository
{
    Task<Place> GetByPlaceIdAsync(string googlePlaceId, string? sessionToken = null);
}