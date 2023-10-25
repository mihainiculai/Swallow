using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swallow.Models
{
    public class User
    {
        public int UserId { get; set; }
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [NotMapped]
        public string FullName { get => $"{FirstName ?? ""} {LastName ?? ""}".Trim(); }
        [MaxLength(100)]
        public required string Email { get; set; }
        public string? Password { get; set; }
        [MaxLength(255)]
        public string? ProfilePictureURL { get; set; }

        public virtual ICollection<Trip> Trips { get; } = new List<Trip>();
        public virtual ICollection<UserPlan> UserPlans { get; } = new List<UserPlan>();
    }
}
