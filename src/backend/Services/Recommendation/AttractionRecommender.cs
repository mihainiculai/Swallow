using Swallow.DTOs.Preference;
using Swallow.Models;
using Swallow.Repositories.Interfaces;

namespace Swallow.Services.Recommendation;

public interface IAttractionRecommender
{
    Task<IEnumerable<Attraction>> GetRecommendations(User user, int cityId, int limit = 5, Trip? trip = null);
}

public class AttractionRecommender(IAttractionRepository attractionRepository,IUserActionRepository userActionRepository) : IAttractionRecommender
{
    private static List<decimal> GetAttractionCategoryScores(IEnumerable<Attraction> attractions, IReadOnlyCollection<UserCategoryPreference> userCategoryPreferences)
    {
        var attractionCategoryScores = new List<decimal>();
        
        foreach (var attraction in attractions)
        {
            decimal attractionCategoryScore = 0;
            var attractionCount = 0;
            
            foreach (var attractionCategory in attraction.AttractionCategories)
            {
                var userCategoryPreference =
                    userCategoryPreferences.FirstOrDefault(ucp => ucp.AttractionCategoryId == attractionCategory.AttractionCategoryId)
                    ?? userCategoryPreferences.FirstOrDefault(ucp => ucp.AttractionCategoryId == 0);

                if (userCategoryPreference == null) continue;
                
                attractionCategoryScore += userCategoryPreference.Score;
                attractionCount++;
            }
            
            attractionCategoryScores.Add(attractionCount == 0 ? 0 : attractionCategoryScore / attractionCount);
        }
        
        return attractionCategoryScores;
    }

    private static (decimal ratingProportion, decimal preferenceProportion) DetermineProportions(int preferenceCount)
    {
        return preferenceCount switch
        {
            0 => (1, 0),
            < 5 => (0.9m, 0.1m),
            < 10 => (0.8m, 0.2m),
            < 20 => (0.7m, 0.3m),
            < 40 => (0.6m, 0.4m),
            _ => (0.5m, 0.5m)
        };
    }
    
    private static List<Attraction> GetAttractionsFromTrip(Trip trip)
    {
        return (from itineraryDay in trip.ItineraryDays from itineraryAttraction in itineraryDay.ItineraryAttractions select itineraryAttraction.Attraction).ToList();
    }

    public async Task<IEnumerable<Attraction>> GetRecommendations(User user, int cityId, int limit = 5, Trip? trip = null)
    {
        var attractions = await attractionRepository.GetByCityIdAndNotDislikedAsync(cityId, user.Id);
    
        if (trip != null)
        {
            var tripAttractions = GetAttractionsFromTrip(trip);
            attractions = attractions.Where(a => !tripAttractions.Contains(a)).ToList();
        }

        var userCategoryPreferences = await userActionRepository.GetNormalizedUserCategoryPreferencesAsync(user);
    
        var categoryScores = GetAttractionCategoryScores(attractions, userCategoryPreferences);
    
        var (ratingProportion, preferenceProportion) = DetermineProportions(await userActionRepository.GetUserPreferenceCountAsync(user));
    
        var combinedScores = attractions.Zip(categoryScores, (attraction, categoryScore) =>
            new
            {
                Attraction = attraction,
                CombinedScore = attraction.NormalizedRating * ratingProportion + categoryScore * preferenceProportion
            });

        var sortedAttractions = combinedScores
            .OrderByDescending(x => x.CombinedScore)
            .Select(x => x.Attraction)
            .Take(limit)
            .ToList();  

        return sortedAttractions;
    }

}