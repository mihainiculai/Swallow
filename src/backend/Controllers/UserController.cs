using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.User;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController(UserManager<User> userManager, ApplicationDbContext context, IUserRepository userRepository) : ControllerBase
    {
        #region ProfilePicture
        [HttpPost("profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                await userRepository.DeleteProfilePictureAsync(User);
            }
            else
            {
                await userRepository.UpdateProfilePictureAsync(User, file);
            }

            return NoContent();
        }
        #endregion

        #region ChangePassword
        [HttpGet("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            return Ok(new { HasPassword = await userRepository.HasPasswordAsync(User) });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            await userRepository.ChangePasswordAsync(User, model.NewPassword, model.CurrentPassword);
            return NoContent();
        }
        #endregion

        #region Profile
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            var user = await userManager.GetUserAsync(User) ?? throw new BadRequestException("User not found.");

            if (model.Email != user.Email)
            {
                if (model.Password == null) throw new BadRequestException("Password is required.");

                if (await userManager.FindByEmailAsync(model.Email) is not null)
                {
                    return Conflict("Email already exists.");
                }
                
                var passwordCheckResult = await userManager.CheckPasswordAsync(user, model.Password);
                if (!passwordCheckResult)
                {
                    return BadRequest("Invalid password.");
                }

                var changeEmailResult = await userManager.ChangeEmailAsync(user, model.Email, model.Password);
                if (!changeEmailResult.Succeeded)
                {
                    return BadRequest(changeEmailResult.Errors);
                }

                user.Email = model.Email;
            }

            user.Public = model.PublicProfile;
            
            if (model.PublicProfile && model.Username == null) return BadRequest("Username is required.");

            if (model.PublicProfile == false) user.PublicUsername = null;

            if (model.Username != null && user.PublicUsername != model.Username)
            {
                if (await context.Users.AnyAsync(u => u.PublicUsername == model.Username))
                {
                    return Conflict("Username already exists.");
                }

                user.PublicUsername = model.Username;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return NoContent();
        }

        [HttpPost("request-delete-account")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequestDto model)
        {
            await userRepository.RequestDeleteAsync(User, model.Password);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("verify-delete-account")]
        public async Task<IActionResult> VerifyDeleteAccount([FromBody] DeleteAccountDto model)
        {
            await userRepository.VerifyDeleteAsync(model.Email, model.Token);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("delete-account")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountDto model)
        {
            await userRepository.DeleteAsync(model.Email, model.Token);
            return NoContent();
        }
        #endregion
    }
}
