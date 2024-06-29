using System.Text.Json.Serialization;

namespace Swallow.DTOs.GoogleMaps;

public class GoogleMapsAutocompleteResponse
{
    [JsonPropertyName("predictions")]
    public List<GoogleMapsAutocompleteResponsePrediction> Predictions { get; set; } = [];
    [JsonPropertyName("status")]
    public string Status { get; set; } = "";
}

public class GoogleMapsAutocompleteResponsePrediction
{
    [JsonPropertyName("structured_formatting")]
    public GoogleMapsAutocompleteResponseStructuredFormatting StructuredFormatting { get; set; } = new();
    [JsonPropertyName("place_id")]
    public string PlaceId { get; set; } = "";
}

public class GoogleMapsAutocompleteResponseStructuredFormatting
{
    [JsonPropertyName("main_text")]
    public string MainText { get; set; } = "";
    [JsonPropertyName("secondary_text")]
    public string SecondaryText { get; set; } = "";
}