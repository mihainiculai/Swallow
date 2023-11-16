using System.ComponentModel.DataAnnotations;

namespace Swallow.Models.DTOs.Authentication
{
    public class LoginModel
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ReCaptchaToken { get; set; }
    }
}
