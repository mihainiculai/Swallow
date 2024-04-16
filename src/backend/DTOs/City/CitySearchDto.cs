namespace Swallow.DTOs.City;

public class CitySearchDto
{
    public int CityId { get; set; }
    public required string Name { get; set; }
    public required string CountryName { get; set; }
    public required string CountryCode { get; set; }
}