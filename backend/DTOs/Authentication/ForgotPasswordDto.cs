using System.ComponentModel.DataAnnotations;

namespace Swallow.DTOs.Authentication
{
    public class ForgotPasswordDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string ReCaptchaToken { get; set; }
    }
}
