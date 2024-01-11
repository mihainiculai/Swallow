using System.ComponentModel.DataAnnotations;

namespace Swallow.Models.DatabaseModels
{
    public class PlatformSettings
    {
        [Key]
        public byte SettingsId { get; set; }
        public required bool MentenanceMode { get; set; }
        public required DateTime NextCurrencyUpdate { get; set; }
    }
}
