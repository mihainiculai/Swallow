using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swallow.Models
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public virtual User User { get; set; } = null!;
    }
}
