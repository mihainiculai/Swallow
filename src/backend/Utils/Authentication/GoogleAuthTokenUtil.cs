using Newtonsoft.Json;
using Swallow.DTOs.Authentication;
using Swallow.Exceptions.CustomExceptions;

namespace Swallow.Utils.Authentication;

public interface IGoogleTokenVerifier
{
    Task<GoogleUserInfo> GetUserInfo(string accessToken);
}

public class GoogleTokenVerifier(HttpClient httpClient, IConfiguration configuration) : IGoogleTokenVerifier
{
    private readonly string _clientId = configuration["Google:ClientId"]!;
    
    public async Task<GoogleUserInfo> GetUserInfo(string accessToken)
    {
        var tokenInfoResponse = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/tokeninfo?access_token={accessToken}");
            
        if (!tokenInfoResponse.IsSuccessStatusCode)
        {
            throw new BadRequestException("Invalid Google access token.");
        }
            
        var tokenInfo = await tokenInfoResponse.Content.ReadAsStringAsync();
        var googleTokenInfo = JsonConvert.DeserializeObject<GoogleTokenInfo>(tokenInfo)!;
            
        if (googleTokenInfo.Aud != _clientId)
        {
            throw new BadRequestException("Invalid Google access token.");
        }
            
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