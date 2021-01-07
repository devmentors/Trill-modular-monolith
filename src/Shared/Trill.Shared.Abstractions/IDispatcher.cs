using System.Threading.Tasks;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Shared.Abstractions
{
    public interface IDispatcher
    {
        Task SendAsync<T>(T command) where T : class, ICommand;
        Task PublishAsync<T>(T @event) where T : class, IEvent;
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
        Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>;
    }
}