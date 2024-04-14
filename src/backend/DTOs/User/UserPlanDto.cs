namespace Swallow.DTOs.User
{
    public class UserPlanDto
    {
        public required string PlanName { get; set; }
        public required DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public required int RemainingTrips { get; set; }
        public required int TotalTrips { get; set; }
        public required bool HasClientPortalAccess { get; set; }
    }
}
