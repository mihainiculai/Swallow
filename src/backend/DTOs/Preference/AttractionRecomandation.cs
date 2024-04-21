namespace Swallow.DTOs.Preference;

public class AttractionRecomandation
{
    public int AttractionId { get; set; }
    public required string Name { get; set; }
    public decimal? Rating { get; set; }
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
}