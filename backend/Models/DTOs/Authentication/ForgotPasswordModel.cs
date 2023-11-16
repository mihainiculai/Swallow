using System.ComponentModel.DataAnnotations;

namespace Swallow.Models.DTOs.Authentication
{
    public class ForgotPasswordModel
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string ReCaptchaToken { get; set; }
    }
}
