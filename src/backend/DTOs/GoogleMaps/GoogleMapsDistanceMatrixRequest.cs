using Newtonsoft.Json;

namespace Swallow.DTOs.GoogleMaps;

public class GoogleMapsDistanceMatrixRequest
{
    [JsonProperty("origins")]
    public string? Origins { get; set; }

    [JsonProperty("destinations")]
    public string? Destinations { get; set; }

    [JsonProperty("departure_time")]
    public string? DepartureTime { get; set; }

    [JsonProperty("arrival_time")]
    public string? ArrivalTime { get; set; }

    [JsonProperty("avoid")]
    public string? Avoid { get; set; }

    [JsonProperty("language")]
    public string? Language { get; set; }

    [JsonProperty("mode")]
    public string? Mode { get; set; }

    [JsonProperty("region")]
    public string? Region { get; set; }

    [JsonProperty("traffic_model")]
    public string? TrafficModel { get; set; }

    [JsonProperty("transit_mode")]
    public string? TransitMode { get; set; }

    [JsonProperty("transit_routing_preference")]
    public string? TransitRoutingPreference { get; set; }

    [JsonProperty("units")]
    public string? Units { get; set; }

    [JsonProperty("key")]
    public string? ApiKey { get; set; }
}