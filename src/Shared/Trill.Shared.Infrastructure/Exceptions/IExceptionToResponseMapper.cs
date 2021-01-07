using System;

namespace Trill.Shared.Infrastructure.Exceptions
{
    internal interface IExceptionToResponseMapper
    {
        ExceptionResponse Map(Exception exception);
    }
}