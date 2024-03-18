using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.City;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations
{
    public class CityRepository(ApplicationDbContext context, IMapper mapper) : ICityRepository
    {
        public async Task<City> GetByIdAsync(int id)
        {
            return await context.Cities.FirstOrDefaultAsync(c => c.CityId == id) ?? throw new NotFoundException("City not found");
        }

        public async Task UpdateAsync(CityDto cityDto)
        {
            var city = await GetByIdAsync(cityDto.CityId);
            city = mapper.Map(cityDto, city);

            context.Cities.Update(city);
            await context.SaveChangesAsync();
        }
    }
}
