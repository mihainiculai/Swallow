namespace Swallow.Models
{
    public class ItineraryDay
    {
        public int ItineraryDayId { get; set; }

        public int TripId { get; set; }
        public virtual Trip Trip { get; set; } = null!;

        public DateOnly Date { get; set; }

        public virtual ICollection<ItineraryAttraction> ItineraryAttractions { get; } = new List<ItineraryAttraction>();
    }
}
