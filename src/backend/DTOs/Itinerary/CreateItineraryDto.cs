namespace Swallow.DTOs.Itinerary;

public class CreateItineraryDto
{
    public required int CityId { get; set; }
    public required int ItineraryType { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required int TransportModeId { get; set; }
    public required string LodgingPlaceId { get; set; }
    public required string LodgingSessionToken { get; set; }
    public required int ArrivingTransportModeId { get; set; }
    public required string ArrivingPlaceId { get; set; }
    public required string ArrivingSessionToken { get; set; }
    public required TimeDetail ArrivingTime { get; set; }
    public required int DepartingTransportModeId { get; set; }
    public required string DepartingPlaceId { get; set; }
    public required string DepartingSessionToken { get; set; }
    public required TimeDetail DepartingTime { get; set; }
}

public class TimeDetail
{
    public required int Hour { get; set; }
    public required int Minute { get; set; }
}