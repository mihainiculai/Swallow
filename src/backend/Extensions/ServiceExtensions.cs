using Swallow.Data;
using Swallow.Services.Currency;
using Swallow.Services.Email;
using Swallow.Utils.FileManagers;
using Swallow.Utils.OpenAi;
using Swallow.Utils.Stripe;

namespace Swallow.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            services.AddScoped<ICurrencyUpdater, CurrencyUpdater>();

            services.Configure<EmailSettings>(services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetSection("EmailSettings"));
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<ILocationDescriptionGenerator, LocationDescriptionGenerator>();
            services.AddScoped<IUserFileManager, UserFileManager>();

            services.AddScoped<IStripeObjects, StripeObjects>();
            services.AddScoped<IStripeCheckout, StripeCheckout>();
            services.AddScoped<IStripeBillingPortal, StripeBillingPortal>();

            services.AddAutoMapper(typeof(Program).Assembly);
        }
    }
}
