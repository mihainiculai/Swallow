using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;
using Swallow.Data;
using Swallow.Mappings;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Implementations;
using Swallow.Repositories.Interfaces;
using Swallow.Services;
using Swallow.Services.Email;
using Swallow.Services.UserManagement;
using Swallow.Utils;
using Swallow.Utils.Authentication;
using System.Net;

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
            services.AddHttpClient<TripAdvisorAttractionsCollector>()
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
                    };
                })
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.AcceptLanguage, "en-GB,en;q=0.9,ro-RO;q=0.8,ro;q=0.7,en-US;q=0.6");
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.AcceptEncoding, "gzip, deflate, br, zstd");
                });
            services.AddHttpClient<ReCaptchaVerifier>();

            services.AddAutoMapper(typeof(AutoMapperProfiles));

            services.AddScoped<IReadOnlyRepository<Country, short>, CountryRepository>();
            services.AddScoped<IReadOnlyRepository<City, int>, CityRepository>();

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
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
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
