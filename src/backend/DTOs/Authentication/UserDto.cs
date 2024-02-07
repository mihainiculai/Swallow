using System.ComponentModel.DataAnnotations;

namespace Swallow.DTOs.Authentication
{
    public class UserDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? ProfilePictureURL { get; set; }
    }
}
