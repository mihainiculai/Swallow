using System.ComponentModel.DataAnnotations;

namespace Swallow.DTOs.Authentication
{
    public class UserDto
    {
        public string? Username { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool? Public { get; set; }
        public int PlanId { get; set; }
    }
}
