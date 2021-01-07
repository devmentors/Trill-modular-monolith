using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Abstractions.Queries;

namespace Trill.Shared.Infrastructure.Queries
{
    internal static class Extensions
    {
        public static IServiceCollection AddQueryHandlers(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection AddInMemoryQueryDispatcher(this IServiceCollection services)
        {
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

            return services;
        }
    }
}