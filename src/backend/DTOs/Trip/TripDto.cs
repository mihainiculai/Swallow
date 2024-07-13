namespace Swallow.DTOs.Trip;

public class TripDto
{
    public Guid TripId { get; set; }
    public required string Destination { get; set; }
    public string? PictureUrl { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
}