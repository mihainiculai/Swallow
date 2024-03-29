﻿using System.Security.Claims;

namespace Swallow.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task ChangePasswordAsync(ClaimsPrincipal claimsPrincipal, string newPassword, string? oldPassword);
        FileStream GetProfilePictureAsync(Guid photoId);
        Task UpdateProfilePictureAsync(ClaimsPrincipal claimsPrincipal, IFormFile file);
        Task DeleteProfilePictureAsync(ClaimsPrincipal claimsPrincipal);
        Task<bool> HasPasswordAsync(ClaimsPrincipal claimsPrincipal);
        Task RequestDeleteAsync(ClaimsPrincipal claimsPrincipal, string password);
        Task VerifyDeleteAsync(string email, string token);
        Task DeleteAsync(string email, string token);
    }
}