using Swallow.DTOs.Attraction;
using Swallow.Models;
using Swallow.Repositories.Interfaces;
using System.Net;
using System.Text.Json;
using Swallow.DTOs.GoogleMaps;

namespace Swallow.Utils.AttractionDataProviders;

public interface IGoogleMapsAttractionsDataFetcher
{
    Task<List<Attraction>> AddAttractionsDetailsAsync(IEnumerable<Attraction> attractions);
}

public class GoogleMapsAttractionsDataFetcher(HttpClient httpClient, IConfiguration configuration,
    IAttractionRepository attractionRepository) : IGoogleMapsAttractionsDataFetcher
{
    private const string GoogleMapsFindPlaceUrl = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json";
    private const string GoogleMapsDetailsUrl = "https://maps.googleapis.com/maps/api/place/details/json";
    private readonly string _googleMapsApiKey = configuration["Google:GoogleMapsApiKey"]!;
        
    private async Task<string?> GetPlaceIdAsync(Attraction attraction)
    {
        var encodedAttractionName = WebUtility.UrlEncode(attraction.Name + ", " + attraction.City.Name);
        var response = await httpClient.GetAsync(GoogleMapsFindPlaceUrl + "?input=" + encodedAttractionName +
                                                 "&inputtype=textquery&locationbias=circle:50000@" + attraction.Latitude + "," + attraction.Longitude +
                                                 "&fields=place_id&key=" + _googleMapsApiKey);

        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var findPlaceResponse = JsonSerializer.Deserialize<GoogleMapsFindPlaceResponse>(json);

        if (findPlaceResponse is null || findPlaceResponse.Status != "OK" || findPlaceResponse.Candidates.Count == 0) return null;

        return findPlaceResponse.Candidates[0].PlaceId;
    }

    private async Task<GoogleMapsDetailsResponseResult?> GetPlaceDetailsAsync(string placeId)
    {
        var response = await httpClient.GetAsync(GoogleMapsDetailsUrl + "?place_id=" + placeId + 
                                                 "&fields=geometry/location,formatted_address,international_phone_number,website,rating,user_ratings_total,url,opening_hours&key=" +
                                                 _googleMapsApiKey);

        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var detailsResponse = JsonSerializer.Deserialize<GoogleMapsDetailsResponse>(json);

        if (detailsResponse is null || detailsResponse.Status != "OK") return null;

        return detailsResponse.Result;
    }
        
    private async Task<Attraction?> AddAttractionDetailsAsync(Attraction attraction)
    {
        if (string.IsNullOrEmpty(_googleMapsApiKey)) throw new Exception("Google Maps API key is not set in the configuration.");

        if (attraction.GooglePlaceId is null)
        {
            attraction.GooglePlaceId = await GetPlaceIdAsync(attraction);
            if (attraction.GooglePlaceId is null) return null;
        }

        var details = await GetPlaceDetailsAsync(attraction.GooglePlaceId);
        if (details is null) return null;

        return await attractionRepository.UpdateAsync(attraction, details);
    }
        
    public async Task<List<Attraction>> AddAttractionsDetailsAsync(IEnumerable<Attraction> attractions)
    {
        List<Attraction> attractionsWithDetails = [];

        foreach (var attraction in attractions)
        {
            try
            {
                var attractionWithDetails = await AddAttractionDetailsAsync(attraction);
                if (attractionWithDetails is not null) attractionsWithDetails.Add(attractionWithDetails);
            }
            catch (Exception)
            {
                // pass
            }
        }

        return attractionsWithDetails;
    }
}