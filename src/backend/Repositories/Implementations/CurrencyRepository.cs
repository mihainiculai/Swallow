using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class CurrencyRepository(ApplicationDbContext context) : ICurrencyRepository
    {
        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await context.Currencies.ToListAsync();
        }

        public async Task<Currency?> GetByCodeAsync(string code)
        {
            return await context.Currencies.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<Currency> GetUSDAsync()
        {
            var currency = await context.Currencies.FirstOrDefaultAsync(c => c.Code == "USD");

            return currency is null ? throw new Exception("USD currency not found") : currency;
        }
    }
}
