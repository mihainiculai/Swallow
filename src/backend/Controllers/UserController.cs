using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.User;
using Swallow.Models.DatabaseModels;

namespace Swallow.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController(UserManager<User> userManager, ApplicationDbContext context) : ControllerBase
    {
        #region ProfilePicture
        const int MAX_PROFILE_PICTURE_SIZE = 5 * 1024 * 1024; // 5 MB

        [HttpGet("profile-picture")]
        public async Task<IActionResult> GetProfilePicture()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null) return BadRequest("User not found.");

            if (user.CustomProfilePicture && user.ProfilePictureURL != null && System.IO.File.Exists(user.ProfilePictureURL))
            {
                var file = System.IO.File.OpenRead(user.ProfilePictureURL);
                return File(file, "image/jpeg");
            }

            return NotFound();
        }

        [HttpPost("profile-picture")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] IFormFile file)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null) return BadRequest("User not found.");

            if (file == null || file.Length == 0) return BadRequest("No file is provided");
            if (!file.ContentType.Contains("image")) return BadRequest("Invalid file type.");
            if (file.Length > MAX_PROFILE_PICTURE_SIZE) return BadRequest("File size is too large. Maximum file size is 5MB.");

            var filePath = Path.Combine("d:/swallow/profile-pictures", $"{Guid.NewGuid()}.jpg");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (user.CustomProfilePicture && user.ProfilePictureURL != null && System.IO.File.Exists(user.ProfilePictureURL))
            {
                System.IO.File.Delete(user.ProfilePictureURL);
            }

            user.ProfilePictureURL = filePath;
            user.CustomProfilePicture = true;

            await userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpDelete("profile-picture")]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null) return BadRequest("User not found.");

            if (user.CustomProfilePicture && user.ProfilePictureURL != null && System.IO.File.Exists(user.ProfilePictureURL))
            {
                System.IO.File.Delete(user.ProfilePictureURL);
            }

            user.ProfilePictureURL = null;
            user.CustomProfilePicture = false;

            await userManager.UpdateAsync(user);

            return NoContent();
        }
        #endregion

        #region ChangePassword
        [HttpGet("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null) return BadRequest("User not found.");

            if (await userManager.HasPasswordAsync(user) == false)
            {
                return Ok(new { HasPassword = false });
            }

            return Ok(new { HasPassword = true });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null) return BadRequest("User not found.");

            if (await userManager.HasPasswordAsync(user) == false)
            {
                var addResult = await userManager.AddPasswordAsync(user, model.NewPassword);

                if (addResult.Succeeded) {
                    return NoContent();
                }

                return BadRequest(addResult.Errors);
            }

            if (model.CurrentPassword == null) return BadRequest("Current password is required.");

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(result.Errors);
        }
        #endregion

        #region UpdateProfile
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null) return BadRequest("User not found.");

            if (model.Email != user.Email)
            {
                if (model.Password == null) return BadRequest("Password is required.");

                if (await userManager.FindByEmailAsync(model.Email) is not null)
                {
                    return Conflict("Email already exists.");
                }

                var changeEmailResult = await userManager.ChangeEmailAsync(user, model.Email, model.Password);

                if (changeEmailResult.Succeeded)
                {
                    user.Email = model.Email;
                }
                else
                {
                    return BadRequest(changeEmailResult.Errors);
                }
            }

            user.Public = model.PublicProfile;
            
            if (model.PublicProfile == true && model.Username == null) return BadRequest("Username is required.");

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
            if (updateResult.Succeeded)
            {
               return NoContent();
            }

            return BadRequest(updateResult.Errors);
        }
        #endregion
    }
}
