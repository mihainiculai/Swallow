namespace Swallow.DTOs.Destination;

public class DestinationDto
{
    public required int CityId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }

    public required string CountryName { get; set; }
    public required string CountryCode { get; set; }
}