namespace Swallow.Models;

public class Trip
{
    public Guid TripId { get; set; }

    public required Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public required int CityId { get; set; }
    public virtual City City { get; set; } = null!;

    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
        
    public virtual TripToHotel? TripToHotel { get; set; }

    public byte TransportModeId { get; set; }
    public virtual TransportMode TransportMode { get; set; } = null!;
    
    public bool IsArchived { get; set; } = false;
    
    public virtual ICollection<Attachment> Attachments { get; } = [];
    public virtual ICollection<ItineraryDay> ItineraryDays { get; } = [];
    public virtual ICollection<TripTransport> TripTransports { get; } = [];
    public virtual ICollection<Expense> Expenses { get; } = [];
}