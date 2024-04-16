using Microsoft.EntityFrameworkCore;
using Stripe;
using Swallow.Data;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class SubscriptionRepository(ApplicationDbContext context) : ISubscriptionRepository
    {
        public async Task<UserPlan> CreateUserSubscription(User user)
        {
            var plan = await context.Plans.FindAsync(1) ?? throw new BadRequestException("User plan can't be created.");
            var dateTime = DateTime.UtcNow;

            UserPlan userPlan = new()
            {
                User = user,
                Plan = plan,
                RemainingTrips = plan.MaxTrips,
                StartDate = dateTime,
                EndDate = dateTime.AddMonths(1)
            };

            await context.UserPlans.AddAsync(userPlan);
            await context.SaveChangesAsync();

            return userPlan;
        }

        private async Task<UserPlan> RenewSubscription(UserPlan lastPlan)
        {
            var userPlan = new UserPlan
            {
                User = lastPlan.User,
                Plan = lastPlan.Plan,
                RemainingTrips = lastPlan.Plan.MaxTrips,
                StartDate = lastPlan.EndDate,
                EndDate = lastPlan.EndDate.AddMonths(1)
            };

            await context.UserPlans.AddAsync(userPlan);
            await context.SaveChangesAsync();

            return userPlan;
        }

        public async Task<int> GetUserSubscription(User user)
        {
            var userPlan = user.UserPlans.Last();

            if (userPlan.EndDate < DateTime.UtcNow)
            {
                userPlan = await RenewSubscription(userPlan);
            }

            return userPlan.PlanId;
        }

        public async Task SetPremiumSubscriptionAsync(Subscription subscription)
        {
            if (subscription.Status == "active")
            {
                return;
            }

            var user = await context.Users.Where(u => u.StripeCustomerId == subscription.CustomerId).FirstAsync();
            var plan = await context.Plans.FindAsync(2);


            var userPlan = new UserPlan()
            {
                PlanId = plan!.PlanId,
                User = user,
                StartDate = subscription.CurrentPeriodStart,
                EndDate = subscription.CurrentPeriodEnd,
                RemainingTrips = plan!.MaxTrips
            };
           
            await context.UserPlans.AddAsync(userPlan);
            await context.SaveChangesAsync();
        }

        public async Task DeleteUserSubscriptionAsync(Subscription subscription)
        {
            var user = await context.Users.Where(u => u.StripeCustomerId == subscription.CustomerId).FirstAsync();

            await CreateUserSubscription(user);
        }   
    }
}
