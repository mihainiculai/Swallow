using System.Net;
using System.Text.Json;
using Swallow.DTOs.GoogleMaps;
using Swallow.Exceptions.CustomExceptions;
using Swallow.Models;

namespace Swallow.Utils.GoogleMaps;

public interface IGoogleMapsSearch
{
    Task<IEnumerable<GoogleMapsAutocompleteResponsePrediction>> AutocompleteSearchPlaceAsync(City city, string input, Guid sessionToken);
}

public class GoogleMapsSearch(HttpClient httpClient, IConfiguration configuration) : IGoogleMapsSearch
{
    private const string GoogleMapsFindPlaceUrl = "https://maps.googleapis.com/maps/api/place/autocomplete/json";
    private readonly string _googleMapsApiKey = configuration["Google:GoogleMapsApiKey"] ?? "";

    public async Task<IEnumerable<GoogleMapsAutocompleteResponsePrediction>> AutocompleteSearchPlaceAsync(City city, string input, Guid sessionToken)
    {
        var encodedInput = WebUtility.UrlEncode(input);
        var url = $"{GoogleMapsFindPlaceUrl}?input={encodedInput}&location={city.Latitude},{city.Longitude}&radius=50000&sessiontoken={sessionToken}&key={_googleMapsApiKey}";
        var response = await httpClient.GetAsync(url);
        
        if (!response.IsSuccessStatusCode) throw new Exception("Google Maps API request failed.");
        
        var json = await response.Content.ReadAsStringAsync();
        var findPlaceResponse = JsonSerializer.Deserialize<GoogleMapsAutocompleteResponse>(json);

        if (findPlaceResponse is null || findPlaceResponse.Status != "OK" || findPlaceResponse.Predictions.Count == 0) throw new NotFoundException("No place found.");
        
        return findPlaceResponse.Predictions;
    }
}