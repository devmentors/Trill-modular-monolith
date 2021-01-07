using System;
using System.Collections.Generic;

namespace Trill.Shared.Abstractions.Messaging
{
    public interface IContract
    {
        Type Type { get; }
        public IEnumerable<string> Required { get; }
    }
}