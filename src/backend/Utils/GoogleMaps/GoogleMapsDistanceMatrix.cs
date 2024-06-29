using Newtonsoft.Json;
using Swallow.DTOs.GoogleMaps;

namespace Swallow.Utils.GoogleMaps;

public interface IGoogleMapsDistanceMatrix
{
    Task<GoogleMapsDistanceMatrixResponse> GetDistanceMatrixAsync(GoogleMapsDistanceMatrixRequest request);
}

public class GoogleMapsDistanceMatrix(HttpClient httpClient, IConfiguration configuration) : IGoogleMapsDistanceMatrix
{
    private readonly string _googleMapsApiKey = configuration["Google:GoogleMapsApiKey"] ?? "";

    public async Task<GoogleMapsDistanceMatrixResponse> GetDistanceMatrixAsync(GoogleMapsDistanceMatrixRequest request)
    {
        var query = $"origins={request.Origins}&destinations={request.Destinations}&key={_googleMapsApiKey}";

        if (!string.IsNullOrEmpty(request.DepartureTime))
            query += $"&departure_time={request.DepartureTime}";

        if (!string.IsNullOrEmpty(request.Avoid))
            query += $"&avoid={request.Avoid}";

        if (!string.IsNullOrEmpty(request.Language))
            query += $"&language={request.Language}";

        if (!string.IsNullOrEmpty(request.Mode))
            query += $"&mode={request.Mode}";

        if (!string.IsNullOrEmpty(request.Region))
            query += $"&region={request.Region}";

        if (!string.IsNullOrEmpty(request.TrafficModel))
            query += $"&traffic_model={request.TrafficModel}";

        if (!string.IsNullOrEmpty(request.TransitMode))
            query += $"&transit_mode={request.TransitMode}";

        if (!string.IsNullOrEmpty(request.TransitRoutingPreference))
            query += $"&transit_routing_preference={request.TransitRoutingPreference}";

        if (!string.IsNullOrEmpty(request.Units))
            query += $"&units={request.Units}";

        var response = await httpClient.GetAsync($"https://maps.googleapis.com/maps/api/distancematrix/json?{query}");
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<GoogleMapsDistanceMatrixResponse>(content)!;
    }
}