using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class CountryRepository(ApplicationDbContext context) : IReadOnlyRepository<Country, int>
    {
        private readonly ApplicationDbContext _context = context;

        public int Count => throw new NotImplementedException();

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == id);
        }
    }
}
