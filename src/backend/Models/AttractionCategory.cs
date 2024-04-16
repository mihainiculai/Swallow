using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class AttractionCategory
    {
        public int AttractionCategoryId { get; set; }
        [MaxLength(60)]
        public required string Name { get; set; }

        public virtual List<Attraction> Attractions { get; } = [];
    }
}
