namespace Swallow.Models
{
    public class TransportMode
    {
        public byte TransportModeId { get; set; }
        public required string Name { get; set; }

        public virtual ICollection<TripTransport> TripTransports { get; } = new List<TripTransport>();
    }
}
