using Microsoft.AspNetCore.Identity;

namespace Swallow.Models
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; } = null!;
    }
}
