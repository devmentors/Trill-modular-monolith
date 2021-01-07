using System.Threading.Tasks;

namespace Trill.Shared.Abstractions.Events
{
    public interface IEventHandler<in TEvent> where TEvent : class, IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}