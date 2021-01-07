namespace Trill.Shared.Kernel.BuildingBlocks
{
    public abstract class AggregateRoot<T>
    {
        public T Id { get; protected set; }
        public int Version { get; protected set; }

        protected AggregateRoot(T id, int version = 0)
        {
            Id = id;
            Version = version;
        }
    }
    
    public abstract class AggregateRoot : AggregateRoot<AggregateId>
    {
        protected AggregateRoot(AggregateId id, int version = 0) : base(id, version)
        {
        }
    }
}