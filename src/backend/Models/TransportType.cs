using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Swallow.Models
{
    public class TransportType
    {
        public byte TransportTypeId { get; set; }
        [MaxLength(40)]
        public required string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<TripTransport> TripTransports { get; } = [];
    }
}
