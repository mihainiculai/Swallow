using System.ComponentModel.DataAnnotations;

namespace Swallow.Models
{
    public class PlatformSettings
    {
        [Key]
        public byte SettingsId { get; set; }
        public required bool MentenanceMode { get; set; }
        public required DateTime NextCurrencyUpdate { get; set; }
    }
}
