using Stripe;
using Swallow.Models;

namespace Swallow.Repositories.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<UserPlan> CreateUserSubscription(User user);
        Task<int> GetUserSubscription(User user);
        Task SetPremiumSubscriptionAsync(Subscription subscription);
        Task DeleteUserSubscriptionAsync(Subscription subscription);
    }
}