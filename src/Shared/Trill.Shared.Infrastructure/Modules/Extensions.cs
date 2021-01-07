using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Abstractions;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Exceptions;
using Trill.Shared.Abstractions.Modules;

namespace Trill.Shared.Infrastructure.Modules
{
    public static class Extensions
    {
        public static IModuleSubscriber UseModuleRequests(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IModuleSubscriber>();
        
        public static IContractRegistry UseContracts(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IContractRegistry>();

        public static IServiceCollection AddExceptionToMessageMapper<T>(this IServiceCollection services)
            where T : class, IExceptionToMessageMapper
        {
            services.AddSingleton<T>();
            services.AddSingleton<IExceptionToMessageMapper, T>();

            return services;
        }
        
        internal static IServiceCollection AddModuleRequests(this IServiceCollection services, IList<Assembly> assemblies)
        {
            services.AddModuleRegistry(assemblies);
            services.AddSingleton<IModuleSubscriber, ModuleSubscriber>();
            services.AddSingleton<IModuleClient, ModuleClient>();
            services.AddSingleton<IContractRegistry, ContractRegistry>();

            return services;
        }

        private static void AddModuleRegistry(this IServiceCollection services, IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(a => a.GetTypes()).ToArray();

            var commandTypes = types
                .Where(t => t.IsClass && typeof(ICommand).IsAssignableFrom(t))
                .ToArray();

            var eventTypes = types
                .Where(t => t.IsClass && typeof(IEvent).IsAssignableFrom(t))
                .ToArray();

            // services.AddSingleton<IModuleSerializer, JsonModuleSerializer>();
            services.AddSingleton<IModuleSerializer, MessagePackModuleSerializer>();
            services.AddSingleton<IModuleRegistry>(sp =>
            {
                var registry = new ModuleRegistry();
                var dispatcher = sp.GetRequiredService<IDispatcher>();
                var dispatcherType = dispatcher.GetType();

                foreach (var type in commandTypes)
                {
                    registry.AddBroadcastAction(type, @event =>
                        (Task) dispatcherType.GetMethod(nameof(dispatcher.SendAsync))
                            ?.MakeGenericMethod(type)
                            .Invoke(dispatcher, new[] {@event}));
                }

                foreach (var type in eventTypes)
                {
                    registry.AddBroadcastAction(type, @event =>
                        (Task) dispatcherType.GetMethod(nameof(dispatcher.PublishAsync))
                            ?.MakeGenericMethod(type)
                            .Invoke(dispatcher, new[] {@event}));
                }

                return registry;
            });
        }
    }
}
