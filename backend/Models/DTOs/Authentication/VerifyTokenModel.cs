using System.ComponentModel.DataAnnotations;

namespace Swallow.Models.DTOs.Authentication
{
    public class VerifyTokenModel
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
