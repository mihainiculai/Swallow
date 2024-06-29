using Newtonsoft.Json;

namespace Swallow.DTOs.GoogleMaps;

public class GoogleMapsDistanceMatrixResponse
{
    [JsonProperty("destination_addresses")]
    public required List<string> DestinationAddresses { get; set; }

    [JsonProperty("origin_addresses")]
    public required List<string> OriginAddresses { get; set; }

    [JsonProperty("rows")]
    public required List<GoogleMapsDistanceMatrixRow> Rows { get; set; }

    [JsonProperty("status")]
    public required string Status { get; set; }

    [JsonProperty("error_message")]
    public string? ErrorMessage { get; set; }
}

public class GoogleMapsDistanceMatrixRow
{
    [JsonProperty("elements")]
    public required List<GoogleMapsDistanceMatrixElement> Elements { get; set; }
}

public class GoogleMapsDistanceMatrixElement
{
    [JsonProperty("status")]
    public required string Status { get; set; }

    [JsonProperty("distance")]
    public GoogleMapsTextValue? Distance { get; set; }

    [JsonProperty("duration")]
    public GoogleMapsTextValue? Duration { get; set; }

    [JsonProperty("duration_in_traffic")]
    public GoogleMapsTextValue? DurationInTraffic { get; set; }

    [JsonProperty("fare")]
    public GoogleMapsFare? Fare { get; set; }
}

public class GoogleMapsTextValue
{
    [JsonProperty("text")]
    public required string Text { get; set; }

    [JsonProperty("value")]
    public required int Value { get; set; }
}

public class GoogleMapsFare
{
    [JsonProperty("currency")]
    public required string Currency { get; set; }

    [JsonProperty("text")]
    public required string Text { get; set; }

    [JsonProperty("value")]
    public required double Value { get; set; }
}