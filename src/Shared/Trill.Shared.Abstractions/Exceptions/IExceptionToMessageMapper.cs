using System;
using System.Collections.Generic;
using Trill.Shared.Abstractions.Events;

namespace Trill.Shared.Abstractions.Exceptions
{
    public interface IExceptionToMessageMapper
    {
        IEnumerable<Type> ExceptionTypes { get; }
        IActionRejected Map<T>(T exception) where T : Exception;
    }
}