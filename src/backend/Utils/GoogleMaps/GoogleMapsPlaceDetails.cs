using Swallow.DTOs.GoogleMaps;
using System.Text.Json;
using Swallow.Exceptions.CustomExceptions;

namespace Swallow.Utils.GoogleMaps;

public interface IGoogleMapsPlaceDetails
{
    Task<GoogleMapsDetailsResponseResult> PlaceDetailsAsync(string placeId, string? sessionToken = null);
}

public class GoogleMapsPlaceDetails(HttpClient httpClient, IConfiguration configuration) : IGoogleMapsPlaceDetails
{
    private const string GoogleMapsDetailsUrl = "https://maps.googleapis.com/maps/api/place/details/json";
    private readonly string _googleMapsApiKey = configuration["Google:GoogleMapsApiKey"] ?? "";

    public async Task<GoogleMapsDetailsResponseResult> PlaceDetailsAsync(string placeId, string? sessionToken = null)
    {
        var url = $"{GoogleMapsDetailsUrl}?place_id={placeId}&fields=geometry/location,name,formatted_address,international_phone_number,website,rating,user_ratings_total,url,opening_hours{(sessionToken is not null ? $"&sessiontoken={sessionToken}" : "")}&key={_googleMapsApiKey}";
        var response = await httpClient.GetAsync(url);
        
        if (!response.IsSuccessStatusCode) throw new Exception("Google Maps API request failed.");
        
        var json = await response.Content.ReadAsStringAsync();
        var findPlaceResponse = JsonSerializer.Deserialize<GoogleMapsDetailsResponse>(json);

        if (findPlaceResponse is null || findPlaceResponse.Status != "OK") throw new NotFoundException("No place found.");

        return findPlaceResponse.Result;
    }
}