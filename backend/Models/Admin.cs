using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        [MaxLength(50)]
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
