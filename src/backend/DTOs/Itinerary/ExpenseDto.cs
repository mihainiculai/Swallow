namespace Swallow.DTOs.Itinerary;

public class ExpenseDto
{
    public Guid ExpenseId { get; set; }
    public Guid TripId { get; set; }
    public short ExpenseCategoryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? AttachmentUrl { get; set; }
    public decimal? Price { get; set; }
    public short? CurrencyId { get; set; }
}