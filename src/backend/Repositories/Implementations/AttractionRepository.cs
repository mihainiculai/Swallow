using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.DTOs.Attraction;
using Swallow.DTOs.GoogleMaps;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Repositories.Implementations;

public class AttractionRepository(ApplicationDbContext context, IAttractionCategoryRepository attractionCategoryRepository,
    ICurrencyRepository currencyRepository, IMapper mapper) : IAttractionRepository
{
    private readonly List<int> _dislikeActionTypes = [3, 4];
        
    public async Task<IEnumerable<Attraction>> GetAllAsync(int? cityId = null)
    {
        if (cityId is not null)
        {
            return await context.Attractions.Where(a => a.CityId == cityId).OrderBy(a => a.Popularity).ToListAsync();
        }

        return await context.Attractions.OrderBy(a => a.Popularity).ToListAsync();
    }

    public async Task<IEnumerable<Attraction>> GetByCityIdAsync(int cityId, int? limit = null)
    {
        if (limit is not null)
        {
            return await context.Attractions.Where(a => a.CityId == cityId).OrderBy(a => a.Popularity).Take((int)limit).ToListAsync();
        }
            
        return await context.Attractions.Where(a => a.CityId == cityId).OrderBy(a => a.Popularity).ToListAsync();
    }
    
    public async Task<IEnumerable<Attraction>> GetByNameAsync(int cityId, string name, int limit = 5)
    {
        return await context.Attractions.Where(a => a.CityId == cityId && a.Name.Contains(name)).OrderByDescending(a => a.NormalizedRating).Take(limit).ToListAsync();
    }
        
    public async Task<List<Attraction>> GetByCityIdAndNotDislikedAsync(int cityId, Guid userId)
    {
        var attractions = await context.Attractions
            .Where(a => a.CityId == cityId && !context.UserActions
                .Where(ua => ua.UserId == userId && _dislikeActionTypes.Contains(ua.UserActionTypeId))
                .Select(ua => ua.AttractionId)
                .Contains(a.AttractionId))
            .ToListAsync();
            
        return attractions;
    }

    public async Task<Attraction?> CreateOrUpdateAsync(TripAdvisorAttraction tripadvisorAttraction, City city, Currency? currency)
    {
        var attraction = city.Attractions.FirstOrDefault(a => a.Name == tripadvisorAttraction.Name);

        var attractionCategories = await attractionCategoryRepository.GetOrCreateAsync(tripadvisorAttraction.Details.Categories);

        currency ??= await currencyRepository.GetUSDAsync();

        if (attraction == null)
        {
            return await CreateAttractionAsync(tripadvisorAttraction, city, currency, attractionCategories);
        }
            
        return await UpdateAsync(attraction, tripadvisorAttraction, currency, attractionCategories);
    }

    public async Task<IEnumerable<Attraction>> CreateOrUpdateAsync(IEnumerable<TripAdvisorAttraction> tripadvisorAttractions, City city)
    {
        await ClearPopularityAsync(city.CityId);

        List<Attraction> attractions = [];

        var currency = await currencyRepository.GetUSDAsync();

        foreach (var attraction in tripadvisorAttractions)
        {
            var addedAttraction = await CreateOrUpdateAsync(attraction, city, currency);

            if (addedAttraction != null)
            {
                attractions.Add(addedAttraction);
            }
        }

        return attractions;
    }

    public async Task<Attraction> CreateAttractionAsync(TripAdvisorAttraction tripAdvisorAttraction, City city, Currency currency, List<AttractionCategory> attractionCategories)
    {
        var attraction = mapper.Map<Attraction>(tripAdvisorAttraction);

        attraction.CityId = city.CityId;
        attraction.Currency = tripAdvisorAttraction.Details.Price != null ? currency : null;
        attraction.AttractionCategories.AddRange(attractionCategories);

        context.Attractions.Add(attraction);
        await context.SaveChangesAsync();

        return attraction;
    }

    public async Task<Attraction> UpdateAsync(Attraction attraction, TripAdvisorAttraction tripAdvisorAttraction, Currency currency, List<AttractionCategory> attractionCategories)
    {
        mapper.Map(tripAdvisorAttraction, attraction);

        attraction.Currency = tripAdvisorAttraction.Details.Price != null ? currency : null;

        attraction.AttractionCategories.Clear();
        attraction.AttractionCategories.AddRange(attractionCategories);

        await context.SaveChangesAsync();

        return attraction;
    }

    public async Task<Attraction> UpdateAsync(Attraction attraction, GoogleMapsDetailsResponseResult googleMapsDetailsResponseResult)
    {
        mapper.Map(googleMapsDetailsResponseResult, attraction);

        await context.SaveChangesAsync();

        if (googleMapsDetailsResponseResult.OpeningHours is not null)
        {
            await UpdateSchedulesAsync(attraction, googleMapsDetailsResponseResult.OpeningHours);
        }

        return attraction;
    }

    public async Task UpdateSchedulesAsync(Attraction attraction, GoogleMapsDetailsResponseResultOpeningHours openingHours)
    {
        attraction.Schedules.Clear();

        var periods = openingHours.Periods;
        foreach (var period in periods)
        {
            Schedule schedule = new()
            {
                AttractionId = attraction.AttractionId,
                WeekdayId = (byte)(period.Open.Day + 1),
                OpenTime = TimeOnly.ParseExact(period.Open.Time, "HHmm"),
                CloseTime = period.Close is not null ? TimeOnly.ParseExact(period.Close.Time, "HHmm") : null,
            };

            attraction.Schedules.Add(schedule);
        }

        await context.SaveChangesAsync();
    }

    public async Task ClearPopularityAsync(int cityId)
    {
        var attractions = await GetByCityIdAsync(cityId);

        foreach (var attraction in attractions)
        {
            attraction.Popularity = null;
        }

        await context.SaveChangesAsync();
    }

    public async Task ClearTrashAsync(City city)
    {
        var attractions = await GetByCityIdAsync(city.CityId);

        foreach (var attraction in attractions)
        {
            if (attraction.Popularity is null)
            {
                context.Attractions.Remove(attraction);
            }

            if (attraction.GooglePlaceId is null)
            {
                context.Attractions.Remove(attraction);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task NormalizeRatingAsync()
    {
        var attractions = await context.Attractions.ToListAsync();
    
        var averageRating = attractions.Average(a => a.Rating ?? 0M);
        var averageReviewCount = attractions.Average(a => a.UserRatingsTotal ?? 0M);

        foreach (var attraction in attractions)
        {
            var rating = attraction.Rating ?? 0M;
            var reviews = attraction.UserRatingsTotal ?? 0M;
    
            var normalizedRating = (reviews / (reviews + averageReviewCount)) * rating + 
                                   (averageReviewCount / (reviews + averageReviewCount)) * averageRating;
    
            attraction.NormalizedRating = normalizedRating;
        }

        await context.SaveChangesAsync();
    }
}