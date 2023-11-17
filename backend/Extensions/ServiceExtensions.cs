using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Swallow.Data;
using Swallow.Models.DatabaseModels;
using Swallow.Services;
using Swallow.Services.Authentication;
using Swallow.Services.Email;
using Swallow.Utils.Authentication;

namespace Swallow.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            //Services
            services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
            services.AddScoped<IErrorLogger, ErrorLogger>();
            services.AddHostedService<UpdateCurrency>();
            services.Configure<EmailSettings>(services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetSection("EmailSettings"));
            services.AddScoped<EmailSender>();
            services.AddScoped<IAuthService, AuthService>();

            //Utils
            services.AddHttpClient<ReCaptchaVerifier>();

            return services;
        }

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.Cookie.Name = "token";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/Identity/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }
    }
}
