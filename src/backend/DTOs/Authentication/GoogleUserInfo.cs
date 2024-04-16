using Newtonsoft.Json;

namespace Swallow.DTOs.Authentication
{
    public class GoogleUserInfo
    {
        [JsonProperty("given_name")]
        public required string GivenName { get; set; }

        [JsonProperty("family_name")]
        public required string FamilyName { get; set; }

        [JsonProperty("picture")]
        public required string Picture { get; set; }

        [JsonProperty("email")]
        public required string Email { get; set; }

        [JsonProperty("email_verified")]
        public required bool EmailVerified { get; set; }
    }
}
