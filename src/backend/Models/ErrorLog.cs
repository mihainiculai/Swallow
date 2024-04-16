using System.ComponentModel.DataAnnotations.Schema;

namespace Swallow.Models
{
    public class ErrorLog
    {
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Message { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? MessageTemplate { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Level { get; set; }

        [Column]
        public DateTime TimeStamp { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Exception { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Properties { get; set; }
    }
}
