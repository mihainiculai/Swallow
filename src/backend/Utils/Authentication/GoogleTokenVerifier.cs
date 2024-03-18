using Newtonsoft.Json;
using Swallow.DTOs.Authentication;
using Swallow.Exceptions.CustomExceptions;

namespace Swallow.Utils.Authentication
{
    public interface IGoogleTokenVerifier
    {
        Task<GoogleUserInfo> GetUserInfo(string accessToken);
    }

    public class GoogleTokenVerifier(HttpClient httpClient) : IGoogleTokenVerifier
    {
        public async Task<GoogleUserInfo> GetUserInfo(string accessToken)
        {
            var userInfoResponse = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={accessToken}");

            if (!userInfoResponse.IsSuccessStatusCode)
            {
                throw new BadRequestException("Invalid Google access token.");
            }

            var userInfo = await userInfoResponse.Content.ReadAsStringAsync();

            var googleUserInfo = JsonConvert.DeserializeObject<GoogleUserInfo>(userInfo)!;

            return googleUserInfo;
        }
    }
}