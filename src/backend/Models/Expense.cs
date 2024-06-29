using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Swallow.Models;

public class Expense
{
    public Guid ExpenseId { get; set; }
    public short ExpenseCategoryId { get; set; }
    public virtual ExpenseCategory ExpenseCategory { get; set; } = null!;
    public Guid TripId { get; set; }
    public virtual Trip Trip { get; set; } = null!;
    [MaxLength(50)]
    public required string Name { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    [MaxLength(255)]
    public string? AttachmentUrl { get; set; }

    [Precision(10, 2)]
    public decimal? Price { get; set; }
    public short? CurrencyId { get; set; }
    public virtual Currency? Currency { get; set; }
    
    public virtual ICollection<TripToHotel> TripToHotels { get; } = [];
    public virtual ICollection<ItineraryAttraction> ItineraryAttractions { get; } = [];
    public virtual ICollection<TripTransport> TripTransports { get; } = [];
}