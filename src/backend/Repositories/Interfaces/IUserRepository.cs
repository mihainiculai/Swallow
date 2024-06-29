using Swallow.Models;
using System.Security.Claims;

namespace Swallow.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task ChangePasswordAsync(ClaimsPrincipal claimsPrincipal, string newPassword, string? oldPassword);
        FileStream GetProfilePictureAsync(Guid photoId);
        UserPlan GetCurrentSubscription(User user);
        Task<UserPlan> GetCurrentSubscription(ClaimsPrincipal claimsPrincipal);
        Task<string> GetStripeClientIdAsync(ClaimsPrincipal claimsPrincipal);
        Task UpdateProfilePictureAsync(ClaimsPrincipal claimsPrincipal, IFormFile file);
        Task DeleteProfilePictureAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> HasPasswordAsync(ClaimsPrincipal claimsPrincipal);
        Task RequestDeleteAsync(ClaimsPrincipal claimsPrincipal, string password);
        Task VerifyDeleteAsync(string email, string token);
        Task DeleteAsync(string email, string token);
    }
}