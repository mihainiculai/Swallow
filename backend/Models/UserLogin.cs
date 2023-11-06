using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Swallow.Models
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; } = null!;
    }
}
