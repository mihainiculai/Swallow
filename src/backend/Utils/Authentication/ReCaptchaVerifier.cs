using Newtonsoft.Json.Linq;

namespace Swallow.Utils.Authentication
{
    public class ReCaptchaVerifier(HttpClient httpClient, IConfiguration configuration)
    {
        private readonly string _reCaptchaSecret = configuration["Google:ReCaptchaSecretKey"] ?? "";

        public async Task<bool> VerifyAsync(string reCaptchaToken)
        {
            var reCaptchaVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";
            var postData = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("secret", _reCaptchaSecret),
            new KeyValuePair<string, string>("response", reCaptchaToken)
            });

            var response = await httpClient.PostAsync(reCaptchaVerificationUrl, postData);
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