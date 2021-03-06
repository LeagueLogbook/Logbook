﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Logbook.Server.Contracts.Social;
using Logbook.Server.Infrastructure.Configuration;
using Logbook.Shared;
using Newtonsoft.Json.Linq;

namespace Logbook.Server.Infrastructure.Social
{
    public class GoogleService : IGoogleService
    {
        public Task<string> GetLoginUrlAsync(string redirectUrl)
        {
            Guard.NotNullOrWhiteSpace(redirectUrl, nameof(redirectUrl));

            string scope = string.Join(" ", Constants.Authentication.GoogleRequiredScopes);

            var url = $"https://accounts.google.com/o/oauth2/v2/auth" +
                      $"?response_type=code" +
                      $"&client_id={Config.Security.GoogleClientId}" +
                      $"&redirect_uri={redirectUrl}" +
                      $"&scope={scope}";

            return Task.FromResult(url);
        }

        public async Task<string> ExchangeCodeForTokenAsync(string redirectUrl, string code)
        {
            Guard.NotNullOrWhiteSpace(redirectUrl, nameof(redirectUrl));
            Guard.NotNullOrWhiteSpace(code, nameof(code));

            var data = new Dictionary<string, string>
            {
                ["code"] = code,
                ["client_id"] = Config.Security.GoogleClientId,
                ["client_secret"] = Config.Security.GoogleClientSecret,
                ["redirect_uri"] = redirectUrl,
                ["grant_type"] = "authorization_code"
            };

            var client = new HttpClient();
            var response = await client.PostAsync("https://www.googleapis.com/oauth2/v4/token", new FormUrlEncodedContent(data));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var responseJsonString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseJsonString);

            return json.Value<string>("access_token");
        }

        public async Task<GoogleUser> GetMeAsync(string token)
        {
            Guard.NotNullOrWhiteSpace(token, nameof(token));

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://www.googleapis.com/plus/v1/people/me");

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var responseJsonString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseJsonString);

            var user = new GoogleUser
            {
                Id = json.Value<string>("id"),
                Locale = json.Value<string>("language"),
                EmailAddress = json.Value<JArray>("emails")
                    ?.Select(f => f.Value<JObject>()?.Value<string>("value"))
                    ?.FirstOrDefault()
            };

            if (string.IsNullOrWhiteSpace(user.Locale) ||
                string.IsNullOrWhiteSpace(user.EmailAddress))
                return null;

            return user;
        }
    }
}