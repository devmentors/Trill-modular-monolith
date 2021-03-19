using System;

namespace Trill.Shared.Abstractions.Time
{
    public interface IClock
    {
        DateTime Current();
    }
}