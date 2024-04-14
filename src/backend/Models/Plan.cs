using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class Plan
    {
        public int PlanId { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }

        public required int MaxTrips { get; set; }
        public required int MaxTripDays { get; set; }
        public required int MaxAttractions { get; set; }
        public required bool ChatbotAccess { get; set; }
        public required bool PersonalizeItinerary { get; set; }
        public required bool PhoneAccess { get; set; }
        public required bool TripTips { get; set; }
        public required bool Ads { get; set; }
        public string? PriceId { get; set; }

        public virtual ICollection<UserPlan> UserPlans { get; } = [];
    }
}
