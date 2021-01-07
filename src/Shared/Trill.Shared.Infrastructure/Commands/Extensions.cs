using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Commands;

namespace Trill.Shared.Infrastructure.Commands
{
    internal static class Extensions
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            services.Scan(s => s.FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>))
                    .WithoutAttribute(typeof(DecoratorAttribute)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IServiceCollection AddInMemoryCommandDispatcher(this IServiceCollection services)
        {
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

            return services;
        }
    }
}