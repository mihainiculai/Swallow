using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swallow.Repositories.Interfaces;

namespace Swallow.Controllers
{
    [Route("files")]
    [ApiController]
    [Authorize]
    public class FilesController(IUserRepository userRepository) : ControllerBase
    {
        [HttpGet("profile-picture")]
        public IActionResult GetProfilePicture([FromQuery] Guid photoId)
        {
            var profilePicture = userRepository.GetProfilePictureAsync(photoId);
            return File(profilePicture, "image/webp");
        }
    }
}
