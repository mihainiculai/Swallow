using System.Text.Json.Serialization;

namespace Swallow.Utils.AttractionDataProviders
{
    public class GoogleMapsFindPlaceResponse
    {
        [JsonPropertyName("candidates")]
        public List<GoogleMapsFindPlaceResponseCandidate> Candidates { get; set; } = [];
        [JsonPropertyName("status")]
        public required string Status { get; set; }
    }

    public class GoogleMapsFindPlaceResponseCandidate
    {
        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; } = "";
    }
}
