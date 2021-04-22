using System;
using System.Collections.Generic;
using Trill.Modules.Users.Core.Domain.Exceptions;
using Trill.Modules.Users.Core.Events;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Exceptions;

namespace Trill.Modules.Users.Core.Exceptions
{
    internal class UsersExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public IEnumerable<Type> ExceptionTypes { get; } =
            new[] {typeof(UserNotFoundException), typeof(InsufficientFundsException)};

        public IActionRejected Map<T>(T exception) where T : Exception
            => exception switch
            {
                UserNotFoundException ex => Map(ex.UserId, ex),
                InsufficientFundsException ex => Map(ex.UserId, ex),
                _ => null
            };

        private static UserActionRejected Map(Guid userId, Exception exception)
            => new(userId, exception.GetExceptionCode(), exception.Message);
    }
}