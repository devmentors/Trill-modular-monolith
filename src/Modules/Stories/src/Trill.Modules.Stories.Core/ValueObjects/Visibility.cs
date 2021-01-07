using System;
using Trill.Modules.Stories.Core.Exceptions;

namespace Trill.Modules.Stories.Core.ValueObjects
{
    internal class Visibility : IEquatable<Visibility>
    {
        public DateTime From { get; }
        public DateTime To { get; }
        public bool Highlighted { get; }

        public Visibility(DateTime from, DateTime to, bool highlighted)
        {
            if (from >= to)
            {
                throw new InvalidVisibilityPeriodException(from, to);
            }
            
            From = from;
            To = to;
            Highlighted = highlighted;
        }

        public static Visibility Default(DateTime from) => new Visibility(from, from.AddDays(7), false);

        public bool Equals(Visibility other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Visibility) obj);
        }

        public override int GetHashCode() => HashCode.Combine(From, To);
    }
}