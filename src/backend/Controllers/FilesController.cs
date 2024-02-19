using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swallow.Models.DatabaseModels;

namespace Swallow.Controllers
{
    [Route("files")]
    [ApiController]
    [Authorize]
    public class FilesController(UserManager<User> userManager) : ControllerBase
    {
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
    }
}
