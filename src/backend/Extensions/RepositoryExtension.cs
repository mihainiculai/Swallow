using Swallow.Repositories.Implementations;
using Swallow.Repositories.Interfaces;

namespace Swallow.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IAttractionCategoryRepository, AttractionCategoryRepository>();
            services.AddScoped<IAttractionRepository, AttractionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        }
    }
}
