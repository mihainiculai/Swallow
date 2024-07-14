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
            services.AddScoped<IUserActionRepository, UserActionRepository>();
            services.AddScoped<IItineraryRepository, ItineraryRepository>();
            services.AddScoped<ITransportRepository, TransportRepository>();
            services.AddScoped<IPlaceRepository, PlaceRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
        }
    }
}
