using System;
using System.Collections.Concurrent;
using System.Net;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Kernel;

namespace Trill.Shared.Infrastructure.Exceptions
{
    internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        private static readonly ConcurrentDictionary<Type, string> Codes = new();

        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => new ExceptionResponse(new {code = GetErrorCode(ex), reason = ex.Message},
                    HttpStatusCode.BadRequest),
                AppException ex => new ExceptionResponse(new {code = GetErrorCode(ex), reason = ex.Message},
                    HttpStatusCode.BadRequest),
                _ => new ExceptionResponse(new {code = "error", reason = "There was an error."},
                    HttpStatusCode.InternalServerError)
            };

        private static string GetErrorCode(object exception)
        {
            var type = exception.GetType();
            return Codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
        }
    }
}