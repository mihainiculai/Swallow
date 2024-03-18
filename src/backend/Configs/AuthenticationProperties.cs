using Microsoft.AspNetCore.Authentication;

namespace Swallow.Configs
{
    public class AuthenticationPropertiesConfig
    {
        public AuthenticationProperties Properties { get; } = new()
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(400),
            AllowRefresh = true
        };
    }
}