using Microsoft.AspNetCore.Identity;

namespace Swallow.Models
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual required Role Role { get; set; }
    }
}
