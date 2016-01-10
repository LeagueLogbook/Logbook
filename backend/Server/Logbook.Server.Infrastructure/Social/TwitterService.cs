using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Logbook.Server.Contracts.Encryption;
using Logbook.Server.Contracts.Social;
using Logbook.Server.Infrastructure.Extensions;
using Newtonsoft.Json.Linq;

namespace Logbook.Server.Infrastructure.Social
{
    public class TwitterService : ITwitterService
    {
        private readonly IEncryptionService _encryptionService;

        public TwitterService(IEncryptionService encryptionService)
        {
            this._encryptionService = encryptionService;
        }

        public async Task<TwitterLoginUrl> GetLoginUrlAsync(string redirectUrl)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth/request_token");

            var nonce = this.CreateNonce();
            var timestamp = this.CreateTimestamp();

            var signature = this.CreateSignature(request, nonce, timestamp, string.Empty, string.Empty, redirectUrl, null);
            this.ApplySignature(request, signature, nonce, timestamp, string.Empty, redirectUrl);

            var client = new HttpClient();
            var response = await  client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;
            
            var responseFormData = await response.Content.ReadAsStringAsync();
            var oauthToken = HttpUtility.ParseQueryString(responseFormData)["oauth_token"];
            var oauthTokenSecret = HttpUtility.ParseQueryString(responseFormData)["oauth_token_secret"];

            return new TwitterLoginUrl
            {
                Url = $"https://api.twitter.com/oauth/authenticate?oauth_token={oauthToken}",
                Payload = this._encryptionService.GenerateForTwitterLogin(oauthToken, oauthTokenSecret)
            };
        }

        public async Task<TwitterToken> ExchangeForToken(string payload, string oauthVerifier)
        {
            var data = new Dictionary<string, string>
            {
                ["oauth_verifier"] = oauthVerifier
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth/access_token");
            request.Content = new FormUrlEncodedContent(data);

            var token = this._encryptionService.DecodeForTwitterLogin(payload);

            var nonce = this.CreateNonce();
            var timestamp = this.CreateTimestamp();

            var signature = this.CreateSignature(request, nonce, timestamp, token.OAuthToken, token.OAuthTokenSecret, string.Empty, data);
            this.ApplySignature(request, signature, nonce, timestamp, token.OAuthToken, string.Empty);

            var client = new HttpClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var responseFormData = await response.Content.ReadAsStringAsync();
            var oauthToken = HttpUtility.ParseQueryString(responseFormData)["oauth_token"];
            var oauthTokenSecret = HttpUtility.ParseQueryString(responseFormData)["oauth_token_secret"];

            return new TwitterToken
            {
                Token = oauthToken,
                Secret = oauthTokenSecret
            };
        }

        public async Task<TwitterUser> GetMeAsync(TwitterToken token)
        {
            var data = new Dictionary<string, string>
            {
                ["include_email"] = "true"
            };

            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.twitter.com/1.1/account/verify_credentials.json?include_email=true");

            var nonce = this.CreateNonce();
            var timestamp = this.CreateTimestamp();

            var signature = this.CreateSignature(request, nonce, timestamp, token.Token, token.Secret, string.Empty, data);
            this.ApplySignature(request, signature, nonce, timestamp, token.Token, string.Empty);

            var client = new HttpClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var responseString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseString);

            return new TwitterUser
            {
                Id = json.Value<string>("id_str"),
                Locale = json.Value<string>("lang"),
                Email = json.Value<string>("email")
            };
        }

        private string CreateNonce()
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString()));
        }

        private string CreateTimestamp()
        {
            return ((int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds).ToString();
        }

        private string CreateSignature(HttpRequestMessage request, string nonce, string timestamp, string token, string tokenSecret, string callback, Dictionary<string, string> additionalData)
        {
            if (additionalData == null)
                additionalData = new Dictionary<string, string>();

            Dictionary<string, string> keysAndValuesDictionary = new Dictionary<string, string>
            {
                { "oauth_callback", callback },
                { "oauth_consumer_key", Config.TwitterConsumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", timestamp },
                { "oauth_token", token },
                { "oauth_version", "1.0" }
            };

            var keysAndValues = new List<KeyValuePair<string, string>>();
            foreach (var pair in keysAndValuesDictionary.ToList().Union(additionalData.ToList()))
            {
                if (string.IsNullOrWhiteSpace(pair.Value))
                    continue;

                keysAndValues.Add(new KeyValuePair<string, string>(Uri.EscapeDataString(pair.Key), Uri.EscapeDataString(pair.Value)));
            }

            keysAndValues = keysAndValues.OrderBy(f => f.Key).ToList();

            StringBuilder output = new StringBuilder();

            foreach (var pair in keysAndValues)
            {
                if (output.Length > 0)
                    output.Append("&");

                output.Append(pair.Key);
                output.Append("=");
                output.Append(pair.Value);
            }

            string baseUrl = $"{request.RequestUri.Scheme}://{request.RequestUri.Host}{request.RequestUri.AbsolutePath}";

            string signatureBaseString = $"{request.Method.Method.ToUpper()}&{Uri.EscapeDataString(baseUrl)}&{Uri.EscapeDataString(output.ToString())}";
            string signingKey = $"{Uri.EscapeDataString(Config.TwitterConsumerSecret)}&{Uri.EscapeDataString(tokenSecret)}";

            var hash = new HMACSHA1(Encoding.ASCII.GetBytes(signingKey));
            byte[] computedHash = hash.ComputeHash(Encoding.ASCII.GetBytes(signatureBaseString));

            return Uri.EscapeDataString(Convert.ToBase64String(computedHash));
        }

        private void ApplySignature(HttpRequestMessage request, string signature, string nonce, string timestamp, string token, string callback)
        {
            Dictionary<string, string> keysAndValuesDictionary = new Dictionary<string, string>
            {
                { "oauth_callback", Uri.EscapeDataString(callback) },
                { "oauth_consumer_key", Config.TwitterConsumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature", signature },
                { "oauth_signature_method", "HMAC-SHA1" },
                { "oauth_timestamp", timestamp },
                { "oauth_token", token },
                { "oauth_version", "1.0" }
            };

            var keysAndValues = keysAndValuesDictionary.ToList().Where(f => string.IsNullOrWhiteSpace(f.Value) == false).OrderBy(f => f.Key).ToList();

            StringBuilder output = new StringBuilder();
            foreach (var pair in keysAndValues)
            {
                output.Append(pair.Key);
                output.Append("=\"");
                output.Append(pair.Value);
                output.Append("\",");
            }

            string header = output.ToString().TrimEnd(',');
            request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", header);
        }
    }
}