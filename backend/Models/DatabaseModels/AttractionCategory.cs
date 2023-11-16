namespace Swallow.Models.DatabaseModels
{
    public class AttractionCategory
    {
        public int AttractionCategoryId { get; set; }
        public required string Name { get; set; }

        public virtual List<Attraction> Attractions { get; } = new();
    }
}
