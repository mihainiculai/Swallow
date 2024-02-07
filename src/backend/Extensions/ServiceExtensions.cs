using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Swallow.Data;
using Swallow.Mappings;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Implementations;
using Swallow.Repositories.Interfaces;
using Swallow.Services;
using Swallow.Services.Email;
using Swallow.Services.UserManagement;
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
            services.AddScoped<IUserService, UserService>();

            //Utils
            services.AddHttpClient<ReCaptchaVerifier>();

            services.AddAutoMapper(typeof(AutoMapperProfiles));

            services.AddScoped<IReadOnlyRepository<Country, int>, CountryRepository>();
            return services;
        }

        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddAuthentication();

            services.AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "token";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                options.User.RequireUniqueEmail = true;
            });

            return services;
        }
    }
}
