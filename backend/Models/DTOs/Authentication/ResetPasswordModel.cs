using System.ComponentModel.DataAnnotations;

namespace Swallow.Models.DTOs.Authentication
{
    public class ResetPasswordModel
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }
        public required string ReCaptchaToken { get; set; }
    }
}
