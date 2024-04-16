using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class ItineraryAttraction
    {
        public int ItineraryAttractionId { get; set; }
        public int ItineraryDayId { get; set; }
        public virtual ItineraryDay ItineraryDay { get; set; } = null!;
        public int AttractionId { get; set; }
        public virtual Attraction Attraction { get; set; } = null!;
        public required int Index { get; set; }

        [Precision(10, 2)]
        public decimal? Price { get; set; }
        public short? CurrencyId { get; set; }
        public virtual Currency? Currency { get; set; }

        [MaxLength(255)]
        public string? TicketsURL { get; set; }
    }
}
