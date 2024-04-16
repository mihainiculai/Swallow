using Microsoft.AspNetCore.Identity;

namespace Swallow.Models
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public virtual User User { get; set; } = null!;
    }
}
