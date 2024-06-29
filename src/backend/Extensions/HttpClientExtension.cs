using Microsoft.Net.Http.Headers;
using Swallow.Utils.AttractionDataProviders;
using Swallow.Utils.Authentication;
using System.Net;
using Swallow.Utils.GoogleMaps;

namespace Swallow.Extensions
{
    public static class HttpClientExtension
    {
        public static void AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<ITripAdvisorAttractionsCollector, TripAdvisorAttractionsCollector>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
                })
                .ConfigureHttpClient(httpClient =>
                {
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36");
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.AcceptLanguage, "en-US,en;q=0.9");
                    httpClient.DefaultRequestHeaders.Add(HeaderNames.AcceptEncoding, "gzip, deflate, br, zstd");
                });
            services.AddHttpClient<IReCaptchaVerifier, ReCaptchaVerifier >();
            services.AddHttpClient<IGoogleMapsAttractionsDataFetcher, GoogleMapsAttractionsDataFetcher>();
            services.AddHttpClient<IGoogleAuthTokenUtil, GoogleAuthTokenUtil>();
            services.AddHttpClient<IGoogleMapsSearch, GoogleMapsSearch>();
            services.AddHttpClient<IGoogleMapsPlaceDetails, GoogleMapsPlaceDetails>();
            services.AddHttpClient<IGoogleMapsDistanceMatrix, GoogleMapsDistanceMatrix>();
        }
    }
}
