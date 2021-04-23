using System;
using System.Collections.Generic;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Exceptions;

namespace Trill.Modules.Ads.Core.Exceptions
{
    internal class AdsExceptionToMessageMapper : IExceptionToMessageMapper
    {
        public IEnumerable<Type> ExceptionTypes { get; }
        
        public IActionRejected Map<T>(T exception) where T : Exception
        {
            throw new NotImplementedException();
        }
    }
}