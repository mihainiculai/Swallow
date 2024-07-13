using Swallow.DTOs.Preference;
using Swallow.Models;

namespace Swallow.Repositories.Interfaces;

public interface IUserActionRepository
{
    Task CreateAsync(Guid userId, int attractionId, int actionType);
    Task<int> GetUserPreferenceCountAsync(User user);
    Task<int> GetUserPreference(User user, int attractionId);
    Task<List<UserCategoryPreference>> GetNormalizedUserCategoryPreferencesAsync(User user);
}