using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Swallow.DTOs.User;
using Swallow.Repositories.Interfaces;
using Swallow.Utils.Stripe;

namespace Swallow.Controllers;

[Authorize]
[Route("api/subscriptions")]
[ApiController]
public class SubscriptionController(IUserRepository userRepository, ISubscriptionRepository subscriptionRepository,
    IStripeObjects stripeObjects, IStripeBillingPortal stripeBillingPortal, IStripeCheckout stripeCheckout,
    IConfiguration configuration, IMapper mapper) : ControllerBase
{
    private readonly string _stripeWebhookSecret = configuration["Stripe:WebhookSecret"]!;

    [AllowAnonymous]
    [HttpGet("price")]
    public async Task<IActionResult> GetPremiumPrice()
    {
        var price = await stripeObjects.GetSubscriptionPriceAsync();

        return Ok(price);
    }

    [HttpGet("current-subscription")]
    public async Task<IActionResult> GetCurrentSubscription()
    {
        var plan = await userRepository.GetCurrentSubscription(User);

        return Ok(mapper.Map<UserPlanDto>(plan));
    }

    [HttpPost("customer-portal")]
    public async Task<IActionResult> GetCustomerPortal()
    {
        var url = await stripeBillingPortal.GetClientPortalUrlAsync(User);

        return Ok(url);
    }

    [HttpPost("create-checkout-session")]
    public async Task<IActionResult> CreateCheckoutSession()
    {
        var url = await stripeCheckout.CreateCheckoutSessionAsync(User);
            
        return Ok(url);
    }

    [AllowAnonymous]
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _stripeWebhookSecret);
            var subscription = stripeEvent.Data.Object as Subscription;

            switch (stripeEvent.Type)
            {
                case Events.CustomerSubscriptionCreated:
                case Events.CustomerSubscriptionUpdated:
                    await subscriptionRepository.SetPremiumSubscriptionAsync(subscription!);
                    break;
                case Events.CustomerSubscriptionDeleted:
                    await subscriptionRepository.DeleteUserSubscriptionAsync(subscription!);
                    break;
            }

            return Ok();
        }
        catch (StripeException)
        {
            return BadRequest();
        }
    }
}