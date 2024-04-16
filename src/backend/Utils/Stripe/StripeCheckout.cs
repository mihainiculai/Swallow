using Stripe.Checkout;
using Stripe;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using System.Security.Claims;

namespace Swallow.Utils.Stripe
{
    public interface IStripeCheckout
    {
        Task<string> CreateCheckoutSessionAsync(ClaimsPrincipal user);
    }

    public class StripeCheckout(IConfiguration configuration, IUserRepository userRepository) : IStripeCheckout
    {
        private readonly string _websiteUrl = configuration["WebsiteURL"]!;
        private readonly string _stripeSecretKey = configuration["Stripe:SecretKey"]!;
        private readonly string _stripePremiumPlanPriceId = configuration["Stripe:PremiumPlanPriceId"]!;

        public async Task<string> CreateCheckoutSessionAsync(ClaimsPrincipal user)
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;

            var options = new SessionCreateOptions
            {
                Mode = "subscription",
                PaymentMethodTypes = ["card"],
                LineItems =
                [
                    new SessionLineItemOptions
                    {
                        Price = _stripePremiumPlanPriceId,
                        Quantity = 1,
                    },
                ],
                Customer = await userRepository.GetStripeClientIdAsync(user),
                SuccessUrl = _websiteUrl + "/settings/membership",
                CancelUrl = _websiteUrl + "/settings/membership",
            };

            var service = new SessionService();
            var session = service.Create(options);

            return session.Url;
        }
    }
}
