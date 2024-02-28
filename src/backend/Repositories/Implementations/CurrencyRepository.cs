using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class CurrencyRepository(ApplicationDbContext context) : IReadOnlyRepository<Currency, short>
    {
        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await context.Currencies.ToListAsync();
        }

        public async Task<Currency?> GetByIdAsync(short id)
        {
            return await context.Currencies.FirstOrDefaultAsync(c => c.CurrencyId == id);
        }

        public async Task<Currency?> GetByCodeAsync(string code)
        {
            return await context.Currencies.FirstOrDefaultAsync(c => c.Code == code);
        }
    }
}
