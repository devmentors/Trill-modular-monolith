using System.Collections.Generic;
using System.Linq;

namespace Trill.Shared.Abstractions.Kernel
{
    public abstract class AggregateRoot<T>
    {
        private bool _versionIncremented;
        private readonly List<IDomainEvent> _events = new();
        
        public IEnumerable<IDomainEvent> Events => _events;
        public T Id { get; protected set; }
        public int Version { get; protected set; }
        
        protected AggregateRoot(T id, int version = 0)
        {
            Id = id;
            Version = version;
        }

        protected void AddEvent(IDomainEvent @event)
        {
            if (!_events.Any() && !_versionIncremented)
            {
                Version++;
                _versionIncremented = true;
            }

            _events.Add(@event);
        }

        public void ClearEvents() => _events.Clear();

        protected void IncrementVersion()
        {
            if (_versionIncremented)
            {
                return;
            }
            
            Version++;
            _versionIncremented = true;
        }
    }
    
    public abstract class AggregateRoot : AggregateRoot<AggregateId>
    {
        protected AggregateRoot(AggregateId id, int version = 0) : base(id, version)
        {
        }
    }
}