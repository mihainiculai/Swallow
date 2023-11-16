using System.ComponentModel.DataAnnotations;

namespace Swallow.Models.DTOs.Authentication
{
    public class RegisterModel
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string ReCaptchaToken { get; set; }
    }
}
