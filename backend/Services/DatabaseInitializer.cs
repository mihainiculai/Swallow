using Swallow.Data;

namespace Swallow.Services
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;

        public DatabaseInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            _context.Database.EnsureCreated();

            if (!_context.Countries.Any())
            {
                await DataInitializer.LoadWorldCities(_context);
            }

            if (!_context.Currencies.Any())
            {
                await DataInitializer.LoadCurrencies(_context);
            }
        }
    }

}
