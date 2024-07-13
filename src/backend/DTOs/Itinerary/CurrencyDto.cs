namespace Swallow.DTOs.Itinerary;

public class CurrencyDto
{
    public short CurrencyId { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Symbol { get; set; }
}