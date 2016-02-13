using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Logbook.Localization;

namespace Logbook.Worker.Api.MessageHandlers
{
    public class LocalizationMessageHandler : DelegatingHandler
    {
        #region Overrides of DelegatingHandler
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.AcceptLanguage != null && request.Headers.AcceptLanguage.Count > 0)
            {
                string language = request.Headers.AcceptLanguage.First().Value;

                if (string.IsNullOrWhiteSpace(language) == false)
                {
                    CultureInfo culture = Languages.GetLanguageByName(language);

                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
        #endregion
    }
}