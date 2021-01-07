using System;
using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Ads.Core.Domain.Exceptions
{
    internal class InvalidPeriodException : DomainException
    {
        public DateTime From { get; }
        public DateTime To { get; }

        public InvalidPeriodException(DateTime from, DateTime to) : base($"Invalid period: '{from}' - '{to}'.")
        {
            From = from;
            To = to;
        }
    }
}