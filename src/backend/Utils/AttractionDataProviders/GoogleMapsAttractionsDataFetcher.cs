using Swallow.Models.DatabaseModels;
using Swallow.Repositories.Implementations;
using System.Net;
using System.Text.Json;

namespace Swallow.Utils.AttractionDataProviders
{
    public class GoogleMapsAttractionsDataFetcher(HttpClient httpClient, IConfiguration configuration, AttractionRepository attractionRepository)
    {
        private static readonly string FindPlaceUrl = "https://maps.googleapis.com/maps/api/place/findplacefromtext/json";
        private static readonly string DetailsUrl = "https://maps.googleapis.com/maps/api/place/details/json";
        private readonly string GoogleMapsApiKey = configuration["Google:GoogleMapsApiKey"] ?? "";

        public async Task<Attraction?> AddAttractionDetails(Attraction attraction)
        {
            if (string.IsNullOrEmpty(GoogleMapsApiKey)) throw new Exception("Google Maps API key is not set in the configuration.");

            if (attraction.GooglePlaceId is null)
            {
                attraction.GooglePlaceId = await GetPlaceId(attraction);
                if (attraction.GooglePlaceId is null) return null;
            }

            GoogleMapsDetailsResponseResult? details = await GetPlaceDetails(attraction.GooglePlaceId);
            if (details is null) return null;

            return await attractionRepository.UpdateAsync(attraction, details);
        }

        public async Task<List<Attraction>> AddAttractionsDetails(IEnumerable<Attraction> attractions)
        {
            List<Attraction> attractionsWithDetails = new();

            foreach (Attraction attraction in attractions)
            {
                Attraction? attractionWithDetails = await AddAttractionDetails(attraction);
                if (attractionWithDetails is not null) attractionsWithDetails.Add(attractionWithDetails);
            }

            return attractionsWithDetails;
        }

        public async Task<string?> GetPlaceId (Attraction attraction)
        {
            var encodedAttractionName = WebUtility.UrlEncode(attraction.Name + ", " + attraction.City.Name);
            var response = await httpClient.GetAsync(FindPlaceUrl + "?input=" + encodedAttractionName + "&inputtype=textquery&locationbias=circle:50000@" + attraction.Latitude + "," + attraction.Longitude + "&fields=place_id&key=" + GoogleMapsApiKey);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var findPlaceResponse = JsonSerializer.Deserialize<GoogleMapsFindPlaceResponse>(json);

            if (findPlaceResponse is null || findPlaceResponse.Status != "OK" || findPlaceResponse.Candidates.Count == 0) return null;

            return findPlaceResponse.Candidates[0].PlaceId;
        }

        public async Task<GoogleMapsDetailsResponseResult?> GetPlaceDetails (string placeId)
        {
            var response = await httpClient.GetAsync(DetailsUrl + "?place_id=" + placeId + "&fields=geometry/location,formatted_address,international_phone_number,website,rating,user_ratings_total,url,opening_hours&key=" + GoogleMapsApiKey);

            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var detailsResponse = JsonSerializer.Deserialize<GoogleMapsDetailsResponse>(json);

            if (detailsResponse is null || detailsResponse.Status != "OK") return null;

            return detailsResponse.Result;
        }

        public static Attraction AddGoogleDetailsToAttraction(Attraction attraction, GoogleMapsDetailsResponseResult details)
        {
            attraction.Latitude = details.Geometry.Location.Latitude;
            attraction.Longitude = details.Geometry.Location.Longitude;
            attraction.Address = details.FormattedAddress;
            attraction.Phone = details.InternationalPhoneNumber;
            attraction.Website = details.Website;
            attraction.Rating = details.Rating;
            attraction.UserRatingsTotal = details.UserRatingsTotal;
            attraction.GoogleMapsUrl = details.Url;

            return attraction;
        }
    }
}
