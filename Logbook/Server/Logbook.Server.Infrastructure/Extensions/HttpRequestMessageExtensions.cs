using System.Net;
using System.Net.Http;
using System.Web.Http;
using JetBrains.Annotations;
using Logbook.Shared.Results;

namespace Logbook.Server.Infrastructure.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        [NotNull]
        public static HttpResponseMessage GetMessageWithError([NotNull]this HttpRequestMessage request, HttpStatusCode statusCode, [CanBeNull]string message)
        {
            return request.CreateErrorResponse(statusCode, new HttpError(message));
        }
        [NotNull]
        public static HttpResponseMessage GetMessageWithResult<T>([NotNull]this HttpRequestMessage request, HttpStatusCode successStatusCode, HttpStatusCode errorStatusCode, [NotNull]Result<T> result, bool ignoreData = false)
        {
            if (result.IsError)
                return request.CreateErrorResponse(errorStatusCode, new HttpError(result.Message));
            else if (ignoreData)
                return request.CreateResponse(successStatusCode);
            else
                return request.CreateResponse(successStatusCode, result.Data);
        }
        [NotNull]
        public static HttpResponseMessage GetMessageWithObject<T>([NotNull]this HttpRequestMessage request, HttpStatusCode statusCode, [NotNull]T obj)
        {
            return request.CreateResponse(statusCode, obj);
        }
        [NotNull]
        public static HttpResponseMessage GetMessage([NotNull]this HttpRequestMessage request, HttpStatusCode statusCode)
        {
            return request.CreateResponse(statusCode);
        }
    }
}