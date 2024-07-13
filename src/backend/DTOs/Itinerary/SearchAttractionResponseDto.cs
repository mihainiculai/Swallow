namespace Swallow.DTOs.Itinerary;

public class SearchAttractionResponseDto
{
    public int AttractionId { get; set; }
    public required string Name { get; set; }
    public string? PictureUrl { get; set; }
}