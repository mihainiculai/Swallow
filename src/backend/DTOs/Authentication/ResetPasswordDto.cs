using System.ComponentModel.DataAnnotations;

namespace Swallow.DTOs.Authentication
{
    public class ResetPasswordDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }
        public required string ReCaptchaToken { get; set; }
    }
}
