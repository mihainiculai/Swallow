using Swallow.DTOs.Attraction;
using Swallow.Models;

namespace Swallow.Repositories.Interfaces
{
    public interface IAttractionRepository
    {
        Task ClearPopularityAsync(int cityId);
        Task ClearTrashAsync(City city);
        Task<Attraction> CreateAttractionAsync(TripAdvisorAttraction tripAdvisorAttraction, City city, Currency currency, List<AttractionCategory> attractionCategories);
        Task<IEnumerable<Attraction>> CreateOrUpdateAsync(IEnumerable<TripAdvisorAttraction> tripadvisorAttractions, City city);
        Task<Attraction?> CreateOrUpdateAsync(TripAdvisorAttraction tripadvisorAttraction, City city, Currency? currency);
        Task<IEnumerable<Attraction>> GetAllAsync(int? cityId = null);
        Task<Attraction> UpdateAsync(Attraction attraction, GoogleMapsDetailsResponseResult googleMapsDetailsResponseResult);
        Task<Attraction> UpdateAsync(Attraction attraction, TripAdvisorAttraction tripAdvisorAttraction, Currency currency, List<AttractionCategory> attractionCategories);
        Task UpdateSchedulesAsync(Attraction attraction, GoogleMapsDetailsResponseResultOpeningHours openingHours);
    }
}