using Stripe;
using Stripe.BillingPortal;
using Swallow.Repositories.Interfaces;
using System.Security.Claims;

namespace Swallow.Utils.Stripe
{
    public interface IStripeBillingPortal
    {
        Task<string> GetClientPortalUrlAsync(ClaimsPrincipal claimsPrincipal);
    }

    public class StripeBillingPortal(IUserRepository userRepository, IConfiguration configuration) : IStripeBillingPortal
    {
        private readonly string _websiteUrl = configuration["WebsiteURL"]!;
        private readonly string _stripeSecretKey = configuration["Stripe:SecretKey"]!;
        private readonly string _stripePremiumPlanPriceId = configuration["Stripe:PremiumPlanPriceId"]!;

        public async Task<string> GetClientPortalUrlAsync(ClaimsPrincipal claimsPrincipal)
        {
            var clientId = await userRepository.GetStripeClientIdAsync(claimsPrincipal);

            StripeConfiguration.ApiKey = _stripeSecretKey;

            var options = new SessionCreateOptions
            {
                Customer = clientId,
                ReturnUrl = _websiteUrl + "/settings/membership",
            };

            var service = new SessionService();
            var session = service.Create(options);

            return session.Url;
        }
    }
}
