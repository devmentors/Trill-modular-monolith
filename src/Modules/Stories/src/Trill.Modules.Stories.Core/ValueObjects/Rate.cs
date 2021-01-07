using System;
using Trill.Modules.Stories.Core.Exceptions;

namespace Trill.Modules.Stories.Core.ValueObjects
{
    internal class Rate : IEquatable<Rate>
    {
        public int Value { get; }

        public Rate(int value)
        {
            if (value < -1 || value > 1)
            {
                throw new InvalidRateException(value);
            }

            Value = value;
        }
        
        public static implicit operator int(Rate rate) => rate.Value;

        public static implicit operator Rate(int rate) => new Rate(rate);

        public bool Equals(Rate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Rate) obj);
        }

        public override int GetHashCode() => Value;
    }
}