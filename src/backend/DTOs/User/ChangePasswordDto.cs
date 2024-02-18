namespace Swallow.DTOs.User
{
    public class ChangePasswordDto
    {
        public string? CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
