using System;
using Trill.Shared.Kernel.Exceptions;

namespace Trill.Modules.Stories.Core.Exceptions
{
    internal class InvalidVisibilityPeriodException : DomainException
    {
        public DateTime From { get; }
        public DateTime To { get; }

        public InvalidVisibilityPeriodException(DateTime from, DateTime to)
            : base($"Invalid visibility period: '{from}' - '{to}'.")
        {
            From = from;
            To = to;
        }
    }
}