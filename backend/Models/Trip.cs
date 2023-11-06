namespace Swallow.Models
{
    public class Trip
    {
        public int TripId { get; set; }

        public required Guid UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public required int CityId { get; set; }
        public virtual City City { get; set; } = null!;

        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set;}

        public virtual ICollection<ItineraryDay> ItineraryDays { get; } = new List<ItineraryDay>();
        public virtual ICollection<TripTransport> TripTransports { get; } = new List<TripTransport>();
        public virtual ICollection<Expense> Expenses { get; } = new List<Expense>();
    }
}
