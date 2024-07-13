namespace Swallow.DTOs.Itinerary;

public class SearchAttractionsDto
{
    public Guid TripId { get; set; }
    public required string Query { get; set; }
}