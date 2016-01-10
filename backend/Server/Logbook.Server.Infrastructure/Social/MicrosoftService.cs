using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Social;
using Logbook.Shared;
using Logbook.Shared.Extensions;
using Newtonsoft.Json.Linq;

namespace Logbook.Server.Infrastructure.Social
{
    public class MicrosoftService : IMicrosoftService
    {
        public Task<string> GetLoginUrlAsync(string redirectUrl)
        {
            var scopes = string.Join(" ", Constants.Authentication.MicrosoftRequiredScopes);

            var url = $"https://login.live.com/oauth20_authorize.srf" +
                      $"?client_id={Config.MicrosoftClientId.GetValue()}" +
                      $"&scope={scopes}" +
                      $"&response_type=code" +
                      $"&redirect_uri={redirectUrl}";

            return Task.FromResult(url);
        }

        public async Task<string> ExchangeCodeForTokenAsync(string redirectUrl, string code)
        {
            var data = new Dictionary<string, string>
            {
                ["client_id"] = Config.MicrosoftClientId,
                ["redirect_uri"] = redirectUrl,
                ["client_secret"] = Config.MicrosoftClientSecret,
                ["code"] = code,
                ["grant_type"] = "authorization_code",
            };

            var client = new HttpClient();
            var response = await client.PostAsync("https://login.live.com/oauth20_token.srf", new FormUrlEncodedContent(data)).WithCurrentCulture();

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var responseJsonString = await response.Content.ReadAsStringAsync().WithCurrentCulture();
            var responseJson = JObject.Parse(responseJsonString);

            var actualScopes = responseJson.Value<string>("scope").Split(' ');

            if (Constants.Authentication.MicrosoftRequiredScopes.All(f => actualScopes.Contains(f)) == false)
                return null;

            return responseJson.Value<string>("access_token");
        }

        public async Task<MicrosoftUser> GetMeAsync(string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://apis.live.net/v5.0/me").WithCurrentCulture();

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var responseJsonString = await response.Content.ReadAsStringAsync().WithCurrentCulture();
            var responseJson = JObject.Parse(responseJsonString);

            var user = new MicrosoftUser
            {
                Id = responseJson.Value<string>("id"),
                EmailAddress = responseJson.Value<JObject>("emails")?.Value<string>("preferred"),
                Locale = responseJson.Value<string>("locale")?.Replace("_", "-")
            };

            if (string.IsNullOrWhiteSpace(user.EmailAddress) ||
                string.IsNullOrWhiteSpace(user.Locale))
                return null;

            return user;
        }
    }
}