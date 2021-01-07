using System;
using Trill.Shared.Abstractions.Events;

namespace Trill.Shared.Abstractions.Exceptions
{
    public interface IExceptionToMessageMapperResolver
    {
        IActionRejected Map<T>(T exception) where T : Exception;
    }
}