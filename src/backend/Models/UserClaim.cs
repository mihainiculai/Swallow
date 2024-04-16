using Microsoft.AspNetCore.Identity;

namespace Swallow.Models
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; } = null!;
    }
}
