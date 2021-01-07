using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Shared.Infrastructure.Queries
{
    internal sealed class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public QueryDispatcher(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            using var scope = _serviceFactory.CreateScope();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            if (handler is null)
            {
                return default;
            }

            return await (Task<TResult>) handlerType
                .GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))?
                .Invoke(handler, new[] {query});

            // dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
            // return await handler.HandleAsync((dynamic) query);
        }

        public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>
        {
            using var scope = _serviceFactory.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            return await handler.HandleAsync(query);
        }
    }
}