using Newtonsoft.Json.Linq;

namespace Swallow.Utils
{
    public class ReCaptchaVerifier
    {
        private readonly HttpClient _httpClient;
        private readonly string _reCaptchaSecret;

        public ReCaptchaVerifier(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _reCaptchaSecret = configuration["Google:ReCaptchaSecretKey"] ?? "";
        }

        public async Task<bool> VerifyAsync(string reCaptchaToken)
        {
            var reCaptchaVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";
            var postData = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("secret", _reCaptchaSecret),
            new KeyValuePair<string, string>("response", reCaptchaToken)
        });

            var response = await _httpClient.PostAsync(reCaptchaVerificationUrl, postData);
            if (response.IsSuccessStatusCode)
            {
                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(jsonResult);
                return (bool)result["success"];
            }

            return false;
        }
    }
}
