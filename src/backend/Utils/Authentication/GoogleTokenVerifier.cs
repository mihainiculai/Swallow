using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;

namespace Swallow.Utils.Authentication
{
    public static class GoogleTokenVerifier
    {
        public static async Task<Userinfo?> VerifyTokenAndGetUserInfo(string accessToken)
        {
            try
            {
                var credential = GoogleCredential.FromAccessToken(accessToken);

                var oauth2Service = new Oauth2Service(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential
                });

                var userInfoRequest = oauth2Service.Userinfo.Get();
                return await userInfoRequest.ExecuteAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
