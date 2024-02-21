using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class CountryRepository(ApplicationDbContext context) : IReadOnlyRepository<Country, short>
    {
        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await context.Countries.ToListAsync();
        }

        public async Task<Country?> GetByIdAsync(short id)
        {
            return await context.Countries.FirstOrDefaultAsync(c => c.CountryId == id);
        }
    }
}
