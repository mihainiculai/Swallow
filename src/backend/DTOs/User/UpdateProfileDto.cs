namespace Swallow.DTOs.User
{
    public class UpdateProfileDto
    {
        public required bool PublicProfile { get; set; }
        public string? Username { get; set; }
        public required string Email { get; set; }
        public string? Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
