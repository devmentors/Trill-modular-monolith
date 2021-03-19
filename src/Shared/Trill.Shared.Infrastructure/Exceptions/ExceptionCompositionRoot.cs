using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Trill.Shared.Infrastructure.Exceptions
{
    internal sealed class ExceptionCompositionRoot : IExceptionCompositionRoot
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ExceptionCompositionRoot(IServiceScopeFactory serviceScopeFactory)
            => _serviceScopeFactory = serviceScopeFactory;

        public ExceptionResponse Map(Exception exception)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>().ToArray();
            var nonDefaultMappers = mappers.Where(x => !(x is ExceptionToResponseMapper));
            var result = nonDefaultMappers
                .Select(x => x.Map(exception))
                .SingleOrDefault(x => x is {});

            if (result is {})
            {
                return result;
            }

            var defaultMapper = mappers.SingleOrDefault(m => m is ExceptionToResponseMapper);
            
            return defaultMapper?.Map(exception);
        }
    }
}