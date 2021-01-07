namespace Trill.Shared.Infrastructure.Mongo
{
    public interface IIdentifiable<out T>
    {
        T Id { get; }
    }
}