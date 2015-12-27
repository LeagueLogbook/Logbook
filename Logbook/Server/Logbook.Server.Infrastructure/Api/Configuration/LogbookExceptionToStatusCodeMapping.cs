using System;
using System.Collections.Generic;
using System.Net;
using Logbook.Server.Infrastructure.Exceptions;

namespace Logbook.Server.Infrastructure.Api.Configuration
{
    public static class LogbookExceptionToStatusCodeMapping
    {
        private static readonly Dictionary<Type, HttpStatusCode> _exceptionToStatusCodeMapping = new Dictionary<Type, HttpStatusCode>
        {
            [typeof(CannotLoginWithPasswordException)] = HttpStatusCode.SeeOther,
            [typeof(EmailIsNotAvailableException)] = HttpStatusCode.BadRequest,
            [typeof(IncorrectPasswordException)] = HttpStatusCode.BadRequest,
            [typeof(InternalServerErrorException)] = HttpStatusCode.InternalServerError,
            [typeof(InvalidJsonWebTokenException)] = HttpStatusCode.BadRequest,
            [typeof(JsonWebTokenTimedOutException)] = HttpStatusCode.Unauthorized,
            [typeof(UnauthorizedException)] = HttpStatusCode.Unauthorized,
            [typeof(UserNotFoundException)] = HttpStatusCode.NotFound,
            [typeof(OnlyLocalException)] = HttpStatusCode.Unauthorized,
            [typeof(DataMissingException)] = HttpStatusCode.BadRequest,
        };

        public static HttpStatusCode GetStatusCode(LogbookException exception)
        {
            HttpStatusCode statusCode;

            if (_exceptionToStatusCodeMapping.TryGetValue(exception.GetType(), out statusCode))
                return statusCode;

            return HttpStatusCode.InternalServerError;
        }
    }
}