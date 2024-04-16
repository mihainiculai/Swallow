using System.ComponentModel.DataAnnotations;

namespace Swallow.DTOs.Authentication
{
    public class VerifyTokenDto
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
