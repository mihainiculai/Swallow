using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class CityRepository(ApplicationDbContext context) : IReadOnlyRepository<City, int>
    {
        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await context.Cities.ToListAsync();
        }

        public async Task<City?> GetByIdAsync(int id)
        {
            return await context.Cities.FirstOrDefaultAsync(c => c.CityId == id);
        }

        public async Task<IEnumerable<City>> GetAllByCountryIdAsync(short countryId)
        {
            return await context.Cities.Where(c => c.CountryId == countryId).ToListAsync();
        }
    }
}
