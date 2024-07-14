namespace Swallow.DTOs.City;

public class CityDiscoverDto
{
    public int CityId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
    public string? TripAdvisorUrl { get; set; }
}