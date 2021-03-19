using System;
using Trill.Shared.Abstractions.Time;

namespace Trill.Shared.Infrastructure.Services
{
    internal class UtcClock : IClock
    {
        public DateTime Current()  => DateTime.UtcNow;
    }
}