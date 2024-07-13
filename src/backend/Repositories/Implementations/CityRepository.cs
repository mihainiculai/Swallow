using AutoMapper;
using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.City;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations;

public class CityRepository(ApplicationDbContext context, IMapper mapper) : ICityRepository
{
    private const int searchThreshold = 80;
        
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

    public async Task CalculateScoreAsync(City city)
    {
        var attractions = await context.Attractions
            .Where(a => a.CityId == city.CityId && a.Popularity > 0)
            .ToListAsync();
            
        foreach (var attraction in city.Attractions)
        {
            city.Score += attraction.Rating * attraction.UserRatingsTotal ?? 0;
        }
            
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<City>> TopCitiesAsync(int? count)
    {
        return await context.Cities
            .Where(c => c.Score > 0)
            .OrderByDescending(c => c.Score)
            .Take(count ?? 5)
            .ToListAsync();
    }

    public IEnumerable<City> Search(string query)
    {
        var cities = context.Cities
            .Include(c => c.Country)
            .Where(c => c.Score > 0)
            .Select(c => new 
            {
                City = c,
                CountryName = c.Country.Name,
                CityName = c.Name
            })
            .AsEnumerable()
            .Select(c => new
            {
                City = c.City,
                Score = Process.ExtractOne(query, new[] { c.CityName, c.CountryName }).Score
            })
            .Where(c => c.Score >= searchThreshold)
            .OrderByDescending(c => c.Score)
            .Take(5)
            .Select(c => c.City)
            .ToList();
            
        return cities;
    }
}