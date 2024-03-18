using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swallow.Models
{
    [Index(nameof(PublicUsername), IsUnique = true)]
    public class User : IdentityUser<Guid>
    {
        [MaxLength(100)]
        public string? PublicUsername { get; set; }
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [NotMapped]
        public string FullName => $"{FirstName ?? ""} {LastName ?? ""}".Trim();
        [Required]
        public bool Public { get; set; } = false;
        [MaxLength(255)]
        public string? GoogleProfilePictureUrl { get; set; }
        [Required]
        public bool HasCustomProfilePicture { get; set; } = false;
        public Guid? ProfilePicturePath { get; set; }

        public virtual ICollection<Trip> Trips { get; } = [];
        public virtual ICollection<UserPlan> UserPlans { get; } = [];

        public virtual ICollection<UserClaim> Claims { get; set; } = [];
        public virtual ICollection<UserLogin> Logins { get; set; } = [];
        public virtual ICollection<UserToken> Tokens { get; set; } = [];
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    }
}