using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swallow.Models
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; } = null!;
    }
}
