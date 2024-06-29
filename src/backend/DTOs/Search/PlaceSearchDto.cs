using Swallow.DTOs.GoogleMaps;

namespace Swallow.DTOs.Search;

public class PlaceSearchDto
{
    public IEnumerable<GoogleMapsAutocompleteResponsePrediction> Predictions { get; set; } = [];
    public Guid SessionToken { get; set; }
}