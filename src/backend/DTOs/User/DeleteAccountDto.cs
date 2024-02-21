namespace Swallow.DTOs.User
{
    public class DeleteAccountDto
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
    }
}
