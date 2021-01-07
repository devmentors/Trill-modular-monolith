using System;
using Trill.Shared.Abstractions;

namespace Trill.Shared.Infrastructure.Services
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Get()  => DateTime.UtcNow;
    }
}