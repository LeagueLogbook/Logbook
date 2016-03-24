using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Logbook.Shared;

namespace Logbook.Worker.Api.Configuration
{
    public class JsonContentNegotiator : IContentNegotiator
    {
        #region Fields
        private readonly JsonMediaTypeFormatter _formatter;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentNegotiator"/> class.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            Guard.NotNull(formatter, nameof(formatter));

            this._formatter = formatter;
        }
        #endregion

        #region Implementation of IContentNegotiator
        /// <summary>
        /// Performs content negotiating by selecting the most appropriate <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> out of the passed in formatters for the given request that can serialize an object of the given type.
        /// </summary>
        /// <param name="type">The type to be serialized.</param>
        /// <param name="request">Request message, which contains the header values used to perform negotiation.</param>
        /// <param name="formatters">The set of <see cref="T:System.Net.Http.Formatting.MediaTypeFormatter" /> objects from which to choose.</param>
        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            var result = new ContentNegotiationResult(this._formatter, new MediaTypeHeaderValue(Constants.HttpApi.ContentType));
            return result;
        }
        #endregion
    }
}