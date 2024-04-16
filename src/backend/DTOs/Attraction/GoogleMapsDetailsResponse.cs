using System.Text.Json.Serialization;

namespace Swallow.DTOs.Attraction
{
    public class GoogleMapsDetailsResponse
    {
        [JsonPropertyName("result")]
        public required GoogleMapsDetailsResponseResult Result { get; set; }
        [JsonPropertyName("status")]
        public required string Status { get; set; }
    }

    public class GoogleMapsDetailsResponseResult
    {
        [JsonPropertyName("geometry")]
        public required GoogleMapsDetailsResponseResultGeometry Geometry { get; set; }
        [JsonPropertyName("formatted_address")]
        public string? FormattedAddress { get; set; }
        [JsonPropertyName("international_phone_number")]
        public string? InternationalPhoneNumber { get; set; }
        [JsonPropertyName("website")]
        public string? Website { get; set; }
        [JsonPropertyName("rating")]
        public decimal? Rating { get; set; }
        [JsonPropertyName("user_ratings_total")]
        public int? UserRatingsTotal { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("opening_hours")]
        public GoogleMapsDetailsResponseResultOpeningHours? OpeningHours { get; set; }
    }

    public class GoogleMapsDetailsResponseResultGeometry
    {
        [JsonPropertyName("location")]
        public required GoogleMapsDetailsResponseResultGeometryLocation Location { get; set; }
    }

    public class GoogleMapsDetailsResponseResultGeometryLocation
    {
        [JsonPropertyName("lat")]
        public required decimal Latitude { get; set; }
        [JsonPropertyName("lng")]
        public required decimal Longitude { get; set; }
    }

    public class GoogleMapsDetailsResponseResultOpeningHours
    {
        [JsonPropertyName("open_now")]
        public bool OpenNow { get; set; }
        [JsonPropertyName("periods")]
        public List<GoogleMapsDetailsResponseResultOpeningHoursPeriod> Periods { get; set; } = [];
    }

    public class GoogleMapsDetailsResponseResultOpeningHoursPeriod
    {
        [JsonPropertyName("open")]
        public required GoogleMapsDetailsResponseResultOpeningHoursPeriodTime Open { get; set; }
        [JsonPropertyName("close")]
        public GoogleMapsDetailsResponseResultOpeningHoursPeriodTime? Close { get; set; }
    }

    public class GoogleMapsDetailsResponseResultOpeningHoursPeriodTime
    {
        [JsonPropertyName("day")]
        public required byte Day { get; set; }
        [JsonPropertyName("time")]
        public required string Time { get; set; }
    }
}
