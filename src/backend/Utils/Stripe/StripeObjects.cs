using Swallow.Models;
using Stripe;

namespace Swallow.Utils.Stripe
{
    public interface IStripeObjects
    {
        Task<string> CreateClientAsync(User user);
        Task<long> GetSubscriptionPriceAsync();
    }

    public class StripeObjects(IConfiguration configuration) : IStripeObjects
    {
        private readonly string _stripeSecretKey = configuration["Stripe:SecretKey"]!;
        private readonly string _stripePremiumPlanPriceId = configuration["Stripe:PremiumPlanPriceId"]!;

        public async Task<long> GetSubscriptionPriceAsync()
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;

            var priceService = new PriceService();
            Price price = await priceService.GetAsync(_stripePremiumPlanPriceId);

            if (price == null || price.UnitAmount == null)
            {
                throw new Exception("Price not found");
            }

            return (long)price.UnitAmount;
        }

        public async Task<string> CreateClientAsync(User user)
        {
            StripeConfiguration.ApiKey = _stripeSecretKey;

            var customerService = new CustomerService();
            var customer = await customerService.CreateAsync(new CustomerCreateOptions
            {
                Email = user.Email,
                Name = user.FullName,
            });

            return customer.Id;
        }
    }
}
