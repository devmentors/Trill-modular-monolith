using System;

namespace Trill.Shared.Infrastructure.Exceptions
{
    internal interface IExceptionCompositionRoot
    {
        ExceptionResponse Map(Exception exception);
    }
}