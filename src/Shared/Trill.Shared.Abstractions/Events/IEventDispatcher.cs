using System.Threading.Tasks;

namespace Trill.Shared.Abstractions.Events
{
    public interface IEventDispatcher
    {
        Task PublishAsync<T>(T @event) where T : class, IEvent;
    }
}