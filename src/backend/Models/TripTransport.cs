using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Swallow.Models.DatabaseModels
{
    [PrimaryKey(nameof(TripId), nameof(TransportModeId))]
    public class TripTransport
    {
        public Guid TripId { get; set; }
        public virtual Trip Trip { get; set; } = null!;
        public byte TransportModeId { get; set; }
        public virtual TransportMode TransportMode { get; set; } = null!;

        public string? TransportNumber { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        [Precision(10, 2)]
        public decimal? Price { get; set; }
        public short? CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; } = null!;

        [MaxLength(255)]
        public string? TicketsURL { get; set; }
    }
}
