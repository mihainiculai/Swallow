using Microsoft.EntityFrameworkCore;

namespace Swallow.Models;

[PrimaryKey(nameof(TripId), nameof(PlaceId))]
public class TripToHotel
{
    public Guid TripId { get; set; }
    public virtual Trip Trip { get; set; } = null!;
    
    public int PlaceId { get; set; }
    public virtual Place Place { get; set; } = null!;
    
    public Guid? ExpenseId { get; set; }
    public virtual Expense? Expense { get; set; }
}