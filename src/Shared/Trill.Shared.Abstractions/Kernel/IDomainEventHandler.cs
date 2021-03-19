using System.Threading.Tasks;

namespace Trill.Shared.Abstractions.Kernel
{
    public interface IDomainEventHandler<in T> where T : class, IDomainEvent
    {
        Task HandleAsync(T domainEvent);
    }
}