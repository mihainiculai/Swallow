using Swallow.DTOs.GoogleMaps;
using Swallow.Models;
using Swallow.Services.Recommendation;
using Swallow.Utils.GoogleMaps;
using Swallow.Utils.Itinerary;

namespace Swallow.Services.Itinerary;

public interface IItineraryAutoCreator
{
    Task<Trip> CreateItineraryAsync(Trip trip);
}

public class ItineraryAutoCreator(IAttractionRecommender attractionRecomandar, IGoogleMapsDistanceMatrix googleMapsDistanceMatrix) : IItineraryAutoCreator
{
    private static string GenerateDepartureTime()
    {
        var today = DateTime.Today;
        var daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)today.DayOfWeek + 7) % 7;

        if (daysUntilWednesday == 0)
        {
            daysUntilWednesday = 7;
        }

        var nextWednesday = today.AddDays(daysUntilWednesday);
        var nextWednesdayAtNoon = new DateTime(nextWednesday.Year, nextWednesday.Month, nextWednesday.Day, 12, 0, 0, DateTimeKind.Utc);

        var unixEpochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var secondsSinceEpoch = (long)(nextWednesdayAtNoon - unixEpochStart).TotalSeconds;

        return secondsSinceEpoch.ToString();
    }
    
    private async Task<List<List<int>>> GetDistanceMatrix(List<Attraction> attractions, Place hotel, int transportMode)
    {
        var places = new List<string> { "place_id:" + hotel.GooglePlaceId };
        places.AddRange(attractions.Select(a => "place_id:" + a.GooglePlaceId));

        var mode = transportMode == 1 ? "transit" : "driving";
        var departureTime = GenerateDepartureTime();
        var distanceMatrix = new List<List<int>>(new int[places.Count].Select(_ => new List<int>(new int[places.Count])));

        var batchSize = 4;
        for (var i = 0; i < places.Count; i += batchSize)
        {
            var originsBatch = places.Skip(i).Take(batchSize).ToList();

            for (var j = 0; j < places.Count; j += batchSize)
            {
                var destinationsBatch = places.Skip(j).Take(batchSize).ToList();

                var request = new GoogleMapsDistanceMatrixRequest
                {
                    Origins = string.Join("|", originsBatch),
                    Destinations = string.Join("|", destinationsBatch),
                    Mode = mode,
                    DepartureTime = departureTime
                };

                var response = await googleMapsDistanceMatrix.GetDistanceMatrixAsync(request);

                for (var k = 0; k < originsBatch.Count; k++)
                {
                    for (var l = 0; l < destinationsBatch.Count; l++)
                    {
                        var originIndex = i + k;
                        var destinationIndex = j + l;

                        if (originIndex < places.Count && destinationIndex < places.Count)
                        {
                            distanceMatrix[originIndex][destinationIndex] = response.Rows[k].Elements[l].Duration != null ? response.Rows[k].Elements[l].Duration!.Value : 300;
                        }
                    }
                }
            }
        }

        return distanceMatrix;
    }

    private (int, TimeOnly) GetFirstDayParams(Trip trip)
    {
        var firstDayTime = trip.TripTransports.First(t => t.TransportRole == TransportRole.Arriving).ArrivalTime!.Value;
        if (firstDayTime.Hour < 23)
        {
            firstDayTime = firstDayTime.AddHours(1);
        }

        return firstDayTime.Hour switch
        {
            < 9 => (4, new TimeOnly(9, 0)),
            < 12 => (3, new TimeOnly(12, 0)),
            < 14 => (2, new TimeOnly(14, 0)),
            < 16 => (1, new TimeOnly(16, 0)),
            _ => (0, new TimeOnly(22, 00)),
        };
    }

    private int GetLastDayParams(Trip trip)
    {
        return trip.TripTransports.First(t => t.TransportRole == TransportRole.Departing).DepartureTime!.Value.Hour switch
        {
            > 21 => 4,
            > 18 => 3,
            > 16 => 2,
            > 14 => 1,
            _ => 0,
        };
    }

    private static void SaveToItineraryDays(Trip trip, int numberOfDays, (int, TimeOnly) firstDayParams,
        List<int> itinerary, List<Attraction> recommendations, int lastDayParams, int numberOfAttractions)
    {
        for (var i = 1; i <= numberOfDays; i++)
        {
            if (i == 1)
            {
                for (var j = 0; j < firstDayParams.Item1; j++)
                {
                    var pos = itinerary[j];
                    var attraction = recommendations[pos - 1];

                    var itineraryAttraction = new ItineraryAttraction
                    {
                        ItineraryDayId = trip.ItineraryDays.ElementAt(i).ItineraryDayId,
                        AttractionId = attraction.AttractionId,
                        Index = j
                    };

                    trip.ItineraryDays.ElementAt(i).ItineraryAttractions.Add(itineraryAttraction);
                }
            }
            else if (i == numberOfDays)
            {
                for (var j = 0; j < lastDayParams; j++)
                {
                    var pos = itinerary[numberOfAttractions - lastDayParams + j];
                    var attraction = recommendations[pos - 1];

                    var itineraryAttraction = new ItineraryAttraction
                    {
                        ItineraryDayId = trip.ItineraryDays.ElementAt(i).ItineraryDayId,
                        AttractionId = attraction.AttractionId,
                        Index = j
                    };

                    trip.ItineraryDays.ElementAt(i).ItineraryAttractions.Add(itineraryAttraction);
                }
            }
            else
            {
                for (var j = 0; j < 4; j++)
                {
                    var pos = itinerary[firstDayParams.Item1 + (i - 2) * 4 + j];
                    var attraction = recommendations[pos - 1];

                    var itineraryAttraction = new ItineraryAttraction
                    {
                        ItineraryDayId = trip.ItineraryDays.ElementAt(i).ItineraryDayId,
                        AttractionId = attraction.AttractionId,
                        Index = j
                    };

                    trip.ItineraryDays.ElementAt(i).ItineraryAttractions.Add(itineraryAttraction);
                }
            }
        }
    }

    public async Task<Trip> CreateItineraryAsync(Trip trip)
    {
        var numberOfDays = trip.ItineraryDays.Count - 1;
        
        var firstDayParams = GetFirstDayParams(trip);
        var lastDayParams = GetLastDayParams(trip);
        var numberOfAttractions = firstDayParams.Item1 + lastDayParams + (numberOfDays - 2) * 4;
        
        var recommendations = (await attractionRecomandar.GetRecommendations(trip.User, trip.CityId, numberOfAttractions) as List<Attraction>)!;
        var distanceMatrix = await GetDistanceMatrix(recommendations, trip.TripToHotel!.Place, trip.TransportModeId);

        var geneticAlgorithm = new ItineraryGeneticAlgorithm([..recommendations], distanceMatrix, numberOfDays, trip.StartDate, firstDayParams.Item1, lastDayParams, firstDayParams.Item2); 
        var itinerary = await Task.Run(() => geneticAlgorithm.GenerateItinerary());
        
        SaveToItineraryDays(trip, numberOfDays, firstDayParams, itinerary, recommendations, lastDayParams, numberOfAttractions);
        
        return trip;
    }
}