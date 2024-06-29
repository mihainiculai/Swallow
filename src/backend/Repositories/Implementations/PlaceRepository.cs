using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Swallow.Data;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using Swallow.Utils.GoogleMaps;

namespace Swallow.Repositories.Implementations;

public class PlaceRepository(ApplicationDbContext context, IGoogleMapsPlaceDetails googleMapsPlaceDetails, IMapper mapper) : IPlaceRepository
{
    private async Task<Place> GetPlaceFromGoogleAsync(string googlePlaceId, string? sessionToken = null)
    {
        var place = await googleMapsPlaceDetails.PlaceDetailsAsync(googlePlaceId, sessionToken);
        
        return mapper.Map<Place>(place);
    }
    
    public async Task<Place> GetByPlaceIdAsync(string googlePlaceId, string? sessionToken = null)
    {
        var place = await context.Places.FirstOrDefaultAsync(p => p.GooglePlaceId == googlePlaceId);
        if (place != null) return place;
        
        place = await GetPlaceFromGoogleAsync(googlePlaceId, sessionToken);
        place.GooglePlaceId = googlePlaceId;
            
        await context.Places.AddAsync(place);
        await context.SaveChangesAsync();

        return place;
    }
}