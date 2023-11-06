using Microsoft.AspNetCore.Identity;

namespace Swallow.Models
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }
    }
}
