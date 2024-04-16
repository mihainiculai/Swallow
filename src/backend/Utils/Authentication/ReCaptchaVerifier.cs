using Newtonsoft.Json.Linq;
using Swallow.Exceptions.CustomExceptions;

namespace Swallow.Utils.Authentication
{
    public interface IReCaptchaVerifier
    {
        Task VerifyAsync(string reCaptchaToken);
    }

    public class ReCaptchaVerifier(HttpClient httpClient, IConfiguration configuration) : IReCaptchaVerifier
    {
        private const string RECAPTCHA_VERIFICATION_URL = "https://www.google.com/recaptcha/api/siteverify";
        private readonly string _reCaptchaSecret = configuration["Google:ReCaptchaSecretKey"] ?? "";

        public async Task VerifyAsync(string reCaptchaToken)
        {
            var postData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("secret", _reCaptchaSecret),
                new KeyValuePair<string, string>("response", reCaptchaToken)
            ]);

            var response = await httpClient.PostAsync(RECAPTCHA_VERIFICATION_URL, postData);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to verify reCaptcha token.");
            }

            var jsonResult = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(jsonResult);
            bool success = (bool)result["success"]!;

            if (!success)
            {
                throw new BadRequestException("Invalid reCaptcha token.");
            }
        }
    }
}