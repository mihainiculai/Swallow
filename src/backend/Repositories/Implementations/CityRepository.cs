using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class CityRepository(ApplicationDbContext context) : IRepository<City, int>
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

        public Task<City> CreateAsync(City entity)
        {
            throw new NotImplementedException();
        }

        public async Task<City?> UpdateAsync(City entity)
        {
            City? city = await GetByIdAsync(entity.CityId);

            if (city == null)
            {
                return null;
            }

            context.Entry(city).CurrentValues.SetValues(entity);
            await context.SaveChangesAsync();

            return city;
        }

        public Task<City?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
