using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logbook.Shared;

namespace Logbook.Worker.Api.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        
        public static HttpResponseMessage GetMessageWithError(this HttpRequestMessage self, HttpStatusCode statusCode, string message)
        {
            Guard.NotNull(self, nameof(self));
            Guard.NotInvalidEnum(statusCode, nameof(statusCode));
            Guard.NotNullOrWhiteSpace(message, nameof(message));

            return self.CreateErrorResponse(statusCode, new HttpError(message));
        }
        
        public static HttpResponseMessage GetMessageWithObject<T>(this HttpRequestMessage self, HttpStatusCode statusCode, T obj)
        {
            Guard.NotNull(self, nameof(self));
            Guard.NotInvalidEnum(statusCode, nameof(statusCode));
            Guard.NotNull(obj, nameof(obj));

            return self.CreateResponse(statusCode, obj);
        }
        
        public static HttpResponseMessage GetMessage(this HttpRequestMessage self, HttpStatusCode statusCode)
        {
            Guard.NotNull(self, nameof(self));
            Guard.NotInvalidEnum(statusCode, nameof(statusCode));

            return self.CreateResponse(statusCode);
        }
    }
}