using Microsoft.AspNetCore.Identity;
using Swallow.Models.DatabaseModels;
using System.Net;
using System.Security.Claims;

namespace Swallow.Services.UserManagement
{
    public interface IUserService
    {
        Task<ServiceResponse<User>> GetCurrentUserAsync(ClaimsPrincipal user);
    }
    public class UserService(UserManager<User> userManager) : IUserService
    {
        private readonly UserManager<User> _userManager = userManager;

        public async Task<ServiceResponse<User>> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            User? currentUser = await _userManager.GetUserAsync(user);
            if (currentUser == null)
            {
                return ServiceResponse<User>.Failure("User not found.", HttpStatusCode.NotFound);
            }
            return ServiceResponse<User>.Success(currentUser);
        }
    }
}
