using System;
using System.Collections.Generic;
using Trill.Modules.Ads.Core.Domain.Exceptions;
using Trill.Modules.Ads.Core.Events;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Exceptions;

namespace Trill.Modules.Ads.Core.Exceptions
{
    internal class AdsExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public IEnumerable<Type> ExceptionTypes { get; } =
            new[] {typeof(AdNotFoundException), typeof(CannotPayAdException), typeof(CannotChangeAdStateException)};

        public IActionRejected Map<T>(T exception) where T : Exception
            => exception switch
            {
                AdNotFoundException ex => Map(ex.AdId, ex),
                CannotPayAdException ex => Map(ex.AdId, ex),
                CannotChangeAdStateException ex => Map(ex.AdId, ex),
                CannotPublishAdAException ex => Map(ex.AdId, ex),
                _ => null
            };

        private static AdActionRejected Map(Guid adId, Exception exception)
            => new AdActionRejected(adId, exception.GetExceptionCode(), exception.Message);
    }
}