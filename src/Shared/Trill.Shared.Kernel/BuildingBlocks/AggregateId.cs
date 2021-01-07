using System;
using System.Collections.Generic;

namespace Trill.Shared.Kernel.BuildingBlocks
{
    public class AggregateId<T> : IEquatable<AggregateId<T>>
    {
        public T Value { get; }

        public AggregateId(T value)
        {
            Value = value;
        }

        public override string ToString() => Value.ToString();

        public bool Equals(AggregateId<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AggregateId<T>) obj);
        }

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Value);
    }

    public class AggregateId : AggregateId<Guid>
    {
        public AggregateId() : this(Guid.NewGuid())
        {
        }
        
        public AggregateId(Guid value) : base(value)
        {
        }

        public static implicit operator Guid(AggregateId id) => id.Value;

        public static implicit operator AggregateId(Guid id) => new AggregateId(id);
    }
}