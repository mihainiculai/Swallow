using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Interfaces;
using Swallow.Utils.AttractionDataProviders;

namespace Swallow.Repositories.Implementations
{
    public class AttractionRepository(ApplicationDbContext context, AttractionCategoryRepository attractionCategoryRepository) : IReadOnlyRepository<Attraction, int>
    {
        public async Task<IEnumerable<Attraction>> GetAllAsync()
        {
            return await context.Attractions.OrderBy(a => a.Popularity).ToListAsync();
        }

        public async Task<Attraction?> GetByIdAsync(int id)
        {
            return await context.Attractions.FirstOrDefaultAsync(a => a.AttractionId == id);
        }

        public async Task<IEnumerable<Attraction>> GetByCityIdAsync(int cityId)
        {
            return await context.Attractions.Where(a => a.CityId == cityId).OrderBy(a => a.Popularity).ToListAsync();
        }

        public async Task<Attraction> CreateAttractionAsync(TripAdvisorAttraction tripAdvisorAttraction, City city, Currency currency, List<AttractionCategory> attractionCategories)
        {
            Attraction attraction = new()
            {
                CityId = city.CityId,
                Name = tripAdvisorAttraction.Name,
                Description = tripAdvisorAttraction.Details.Description,
                Popularity = tripAdvisorAttraction.Popularity,
                TripAdvisorUrl = tripAdvisorAttraction.TripAdvisorLink,
                PictureUrl = tripAdvisorAttraction.Details.ImageUrl,
                VisitDuration = tripAdvisorAttraction.Details.VisitDuration,
                Price = tripAdvisorAttraction.Details.Price,
                Currency = tripAdvisorAttraction.Details.Price != null ? currency : null,
            };

            attraction.AttractionCategories.AddRange(attractionCategories);

            context.Attractions.Add(attraction);
            await context.SaveChangesAsync();

            return attraction;
        }

        public async Task<Attraction?> CreateOrUpdateAsync(TripAdvisorAttraction tripadvisorAttraction, City city, Currency currency)
        {
            Attraction? attraction = city.Attractions.FirstOrDefault(a => a.Name == tripadvisorAttraction.Name);

            List<AttractionCategory> attractionCategories = await attractionCategoryRepository.GetOrCreateAsync(tripadvisorAttraction.Details.Categories);

            if (attraction == null)
            {
                return await CreateAttractionAsync(tripadvisorAttraction, city, currency, attractionCategories);
            }
            else
            {
                return await UpdateAsync(attraction, tripadvisorAttraction, currency, attractionCategories);
            }
        }

        public async Task<IEnumerable<Attraction>> CreateOrUpdateAsync(IEnumerable<TripAdvisorAttraction> tripadvisorAttractions, City city, Currency currency)
        {
            await RemovePopularity(city.CityId);

            List<Attraction> attractions = [];

            foreach (TripAdvisorAttraction attraction in tripadvisorAttractions)
            {
                Attraction? addedAttraction = await CreateOrUpdateAsync(attraction, city, currency);

                if (addedAttraction != null)
                {
                    attractions.Add(addedAttraction);
                }
            }

            return attractions;
        }

        public async Task<Attraction> UpdateAsync(Attraction attraction, TripAdvisorAttraction tripAdvisorAttraction, Currency currency, List<AttractionCategory> attractionCategories)
        {
            attraction.Popularity = tripAdvisorAttraction.Popularity;
            attraction.Description = tripAdvisorAttraction.Details.Description;
            attraction.TripAdvisorUrl = tripAdvisorAttraction.TripAdvisorLink;
            attraction.PictureUrl = tripAdvisorAttraction.Details.ImageUrl;
            attraction.VisitDuration = tripAdvisorAttraction.Details.VisitDuration;
            attraction.Price = tripAdvisorAttraction.Details.Price;
            attraction.Currency = tripAdvisorAttraction.Details.Price != null ? currency : null;

            attraction.AttractionCategories.Clear();
            attraction.AttractionCategories.AddRange(attractionCategories);

            await context.SaveChangesAsync();

            return attraction;
        }

        public async Task<Attraction> UpdateAsync(Attraction attraction, GoogleMapsDetailsResponseResult googleMapsDetailsResponseResult)
        {
            attraction.Latitude = googleMapsDetailsResponseResult.Geometry.Location.Latitude;
            attraction.Longitude = googleMapsDetailsResponseResult.Geometry.Location.Longitude;
            attraction.Address = googleMapsDetailsResponseResult.FormattedAddress;
            attraction.Phone = googleMapsDetailsResponseResult.InternationalPhoneNumber;
            attraction.Website = googleMapsDetailsResponseResult.Website;
            attraction.Rating = googleMapsDetailsResponseResult.Rating;
            attraction.UserRatingsTotal = googleMapsDetailsResponseResult.UserRatingsTotal;
            attraction.GoogleMapsUrl = googleMapsDetailsResponseResult.Url;

            if (googleMapsDetailsResponseResult.OpeningHours is not null && googleMapsDetailsResponseResult.OpeningHours.Periods is not null)
            {
                attraction.Schedules.Clear();

                var periods = googleMapsDetailsResponseResult.OpeningHours.Periods;
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
            }

            await context.SaveChangesAsync();

            return attraction;
        }

        public async Task RemovePopularity(int cityId)
        {
            IEnumerable<Attraction> attractions = await GetByCityIdAsync(cityId);

            foreach (Attraction attraction in attractions)
            {
                attraction.Popularity = null;
            }

            await context.SaveChangesAsync();
        }

        public async Task ClearCityAsync(City city)
        {
            IEnumerable<Attraction> attractions = await GetByCityIdAsync(city.CityId);

            foreach (Attraction attraction in attractions)
            {
                if (attraction.Popularity is null)
                {
                    context.Attractions.Remove(attraction);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
