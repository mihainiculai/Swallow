using Newtonsoft.Json;

namespace Swallow.DTOs.Currency;

public class CurrencyApiResponse
{
    [JsonProperty("time_last_update_utc")]
    public required string TimeLastUpdateUtc { get; set; }
    [JsonProperty("time_next_update_utc")]
    public required string TimeNextUpdateUtc { get; set; }
    [JsonProperty("rates")]
    public required Dictionary<string, decimal> Rates { get; set; }
}