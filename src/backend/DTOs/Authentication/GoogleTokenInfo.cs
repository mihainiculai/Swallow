using Newtonsoft.Json;

namespace Swallow.DTOs.Authentication;

public class GoogleTokenInfo
{
    [JsonProperty("aud")]
    public required string Aud { get; set; }
}