using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.Country;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class CountryRepository(ApplicationDbContext context, IMapper mapper) : ICountryRepository
    {
        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await context.Countries.ToListAsync();
        }

        public async Task<Country> GetByIdAsync(short id)
        {
            return await context.Countries.FirstOrDefaultAsync(c => c.CountryId == id) ?? throw new NotFoundException();
        }

        public async Task UpdateAsync(CountryDetailsDto cityDto)
        {
            var country = await GetByIdAsync(cityDto.CountryId);
            country = mapper.Map(cityDto, country);

            context.Countries.Update(country);
            await context.SaveChangesAsync();
        }
    }
}
