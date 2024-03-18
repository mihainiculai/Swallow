using Microsoft.AspNetCore.Identity;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using Swallow.Services.Email;
using Swallow.Utils.FileManagers;
using System.Security.Claims;

namespace Swallow.Repositories.Implementations
{
    public class UserRepository(UserManager<User> userManager, IUserFileManager userFileManager, IEmailSender emailSender) : IUserRepository
    {
        private async Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await userManager.GetUserAsync(claimsPrincipal) ?? throw new BadRequestException("User not found.");
        }

        public async Task<bool> HasPasswordAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await GetUserAsync(claimsPrincipal);
            return await userManager.HasPasswordAsync(user);
        }
        private async Task<bool> HasPasswordAsync(User user)
        {
            return await userManager.HasPasswordAsync(user);
        }

        public async Task ChangePasswordAsync(ClaimsPrincipal claimsPrincipal, string newPassword, string? oldPassword)
        {
            var user = await GetUserAsync(claimsPrincipal);

            if (!await HasPasswordAsync(user))
            {
                await AddPasswordAsync(user, newPassword);
            }
            else
            {
                if (oldPassword == null)
                {
                    throw new BadRequestException("Old password is required.");
                }

                await UpdatePasswordAsync(user, oldPassword, newPassword);
            }
        }
        private async Task AddPasswordAsync(User user, string newPassword)
        {
            var result = await userManager.AddPasswordAsync(user, newPassword);

            if (!result.Succeeded)
            {
                throw new BadRequestException("Failed to add password.");
            }
        }
        private async Task UpdatePasswordAsync(User user, string oldPassword, string newPassword)
        {
            var passwordCheckResult = await userManager.CheckPasswordAsync(user, oldPassword);
            if (!passwordCheckResult)
            {
                throw new BadRequestException("Invalid current password.");
            }

            var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                throw new BadRequestException("Failed to change password.");
            }
        }

        public FileStream GetProfilePictureAsync(Guid photoId)
        {
            var profilePicture = userFileManager.GetProfilePicture(photoId);

            return profilePicture;
        }
        public async Task UpdateProfilePictureAsync(ClaimsPrincipal claimsPrincipal, IFormFile file)
        {
            var user = await GetUserAsync(claimsPrincipal);

            user.ProfilePicturePath = await userFileManager.SaveProfilePicture(user, file);
            user.HasCustomProfilePicture = true;

            await userManager.UpdateAsync(user);
        }
        public async Task DeleteProfilePictureAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await GetUserAsync(claimsPrincipal);
            userFileManager.DeleteProfilePicture(user);

            user.ProfilePicturePath = null;
            await userManager.UpdateAsync(user);
        }

        public async Task RequestDeleteAsync(ClaimsPrincipal claimsPrincipal, string password)
        {
            var user = await GetUserAsync(claimsPrincipal);

            var passwordCheckResult = await userManager.CheckPasswordAsync(user, password);

            if (!passwordCheckResult)
            {
                throw new BadRequestException("Invalid password.");
            }

            var token = await userManager.GenerateUserTokenAsync(user, "Default", "DeleteAccount");
            await emailSender.SendAccountDeletionEmailAsync(user.Email!, user.FullName, token);
        }
        public async Task VerifyDeleteAsync(string email, string token)
        {

            var user = await userManager.FindByEmailAsync(email) ?? throw new BadRequestException("User not found.");
            var result = await userManager.VerifyUserTokenAsync(user, "Default", "DeleteAccount", token);

            if (!result)
            {
                throw new BadRequestException("Invalid token.");
            }
        }
        public async Task DeleteAsync(string email, string token) {
            var user = await userManager.FindByEmailAsync(email) ?? throw new BadRequestException("User not found.");
            var result = await userManager.VerifyUserTokenAsync(user, "Default", "DeleteAccount", token);

            if (!result)
            {
                throw new BadRequestException("Invalid token.");
            }

            await userManager.DeleteAsync(user);
            userFileManager.DeleteProfilePicture(user);
        }
    }
}
