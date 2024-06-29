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
        
        public Guid? ExpenseId { get; set; }
        public virtual Expense? Expense { get; set; }

        [MaxLength(255)]
        public string? TicketsUrl { get; set; }
    }
}
