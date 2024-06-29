using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Swallow.Models;

public class TransportMode
{
    public byte TransportModeId { get; set; }
    [MaxLength(40)]
    public string Name { get; set; } = null!;
    
    [JsonIgnore]
    public virtual ICollection<Trip> Trips { get; } = [];
}