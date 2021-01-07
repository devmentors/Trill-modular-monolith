using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Events;

namespace Trill.Shared.Infrastructure.Events
{
    internal static class Extensions
    {
        public static IServiceCollection AddEventHandlers(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))
                    .WithoutAttribute(typeof(DecoratorAttribute)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection AddInMemoryEventDispatcher(this IServiceCollection services)
        {
            services.AddSingleton<IEventDispatcher, EventDispatcher>();

            return services;
        }
    }
}