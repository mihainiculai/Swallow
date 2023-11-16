using Swallow.Models.DatabaseModels;
using Swallow.Models.DTOs.Authentication;

namespace Swallow.Utils.Authentication
{
    public class UserResponse
    {
        public static Response Create(User user)
        {
            return new Response
            {
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                ProfilePictureURL = user.ProfilePictureURL
            };
        }
    }
}
