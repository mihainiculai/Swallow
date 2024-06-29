using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class TransportType
    {
        public byte TransportModeId { get; set; }
        [MaxLength(40)]
        public required string Name { get; set; }

        public virtual ICollection<TripTransport> TripTransports { get; } = [];
    }
}
