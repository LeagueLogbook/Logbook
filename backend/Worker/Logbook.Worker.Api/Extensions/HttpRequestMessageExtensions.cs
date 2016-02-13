using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Logbook.Worker.Api.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        
        public static HttpResponseMessage GetMessageWithError(this HttpRequestMessage request, HttpStatusCode statusCode, string message)
        {
            return request.CreateErrorResponse(statusCode, new HttpError(message));
        }
        
        public static HttpResponseMessage GetMessageWithObject<T>(this HttpRequestMessage request, HttpStatusCode statusCode, T obj)
        {
            return request.CreateResponse(statusCode, obj);
        }
        
        public static HttpResponseMessage GetMessage(this HttpRequestMessage request, HttpStatusCode statusCode)
        {
            return request.CreateResponse(statusCode);
        }
    }
}