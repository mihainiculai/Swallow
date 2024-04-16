namespace Swallow.DTOs.Destination;

public class DestinationAttractionDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Rating { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public List<string> Categories { get; set; } = [];
    public string? VisitDuration { get; set; }
    public string? TripAdvisorUrl { get; set; }
    public string? GoogleMapsUrl { get; set; }
    public string? PictureUrl { get; set; }
}