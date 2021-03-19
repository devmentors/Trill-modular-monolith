using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Events;

namespace Trill.Shared.Infrastructure.Events
{
    internal static class Extensions
    {
        public static IServiceCollection AddEvents(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.AddSingleton<IEventDispatcher, EventDispatcher>();
            services.Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))
                    .WithoutAttribute(typeof(DecoratorAttribute)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}