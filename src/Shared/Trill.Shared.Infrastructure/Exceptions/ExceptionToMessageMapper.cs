using System;
using System.Collections.Generic;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Exceptions;
using Trill.Shared.Infrastructure.Messaging;
using Trill.Shared.Kernel.Exceptions;

namespace Trill.Shared.Infrastructure.Exceptions
{
    internal sealed class ExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public IEnumerable<Type> ExceptionTypes { get; } = new []{typeof(AppException), typeof(DomainException), typeof(Exception)};

        public IActionRejected Map<T>(T exception) where T : Exception
            => exception switch
            {
                AppException ex => new ActionRejected(ex.GetExceptionCode(), ex.Message),
                DomainException ex => new ActionRejected(ex.GetExceptionCode(), ex.Message),
                _ => new ActionRejected("error", "There was an error.")
            };
    }
}