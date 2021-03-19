using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Figgle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Trill.Shared.Abstractions.Commands;
using Trill.Shared.Abstractions.Dispatchers;
using Trill.Shared.Abstractions.Events;
using Trill.Shared.Abstractions.Exceptions;
using Trill.Shared.Abstractions.Generators;
using Trill.Shared.Abstractions.Queries;
using Trill.Shared.Abstractions.Storage;
using Trill.Shared.Abstractions.Time;
using Trill.Shared.Infrastructure.Api;
using Trill.Shared.Infrastructure.Auth;
using Trill.Shared.Infrastructure.Commands;
using Trill.Shared.Infrastructure.Contexts;
using Trill.Shared.Infrastructure.Dispatchers;
using Trill.Shared.Infrastructure.Events;
using Trill.Shared.Infrastructure.Exceptions;
using Trill.Shared.Infrastructure.Generators;
using Trill.Shared.Infrastructure.Kernel;
using Trill.Shared.Infrastructure.Logging;
using Trill.Shared.Infrastructure.Messaging;
using Trill.Shared.Infrastructure.Messaging.Dispatchers;
using Trill.Shared.Infrastructure.Modules;
using Trill.Shared.Infrastructure.Mongo;
using Trill.Shared.Infrastructure.Queries;
using Trill.Shared.Infrastructure.Redis;
using Trill.Shared.Infrastructure.Services;
using Trill.Shared.Infrastructure.Storage;

[assembly: InternalsVisibleTo("Trill.Bootstrapper")]
[assembly: InternalsVisibleTo("Trill.Tests.Benchmarks")]
[assembly: InternalsVisibleTo("Trill.Shared.Tests.Integration")]
namespace Trill.Shared.Infrastructure
{
    internal static class Extensions
    {
        private const string CorsPolicy = "cors";
        private const string AppSectionName = "app";
        
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IList<Assembly> assemblies,IList<IModule> modules, string sectionName = AppSectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = AppSectionName;
            }

            var disabledModules = new List<string>();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                foreach (var (key, value) in configuration.AsEnumerable())
                {
                    if (!key.Contains(":module:enabled"))
                    {
                        continue;
                    }

                    if (!bool.Parse(value))
                    {
                        disabledModules.Add(key.Split(":")[0]);
                    }
                }
            }

            services
                .AddCommands(assemblies)
                .AddEvents(assemblies)
                .AddQueries(assemblies)
                .AddDomainEvents(assemblies)
                .AddMessaging()
                .AddModuleRequests(assemblies)
                .AddMongo()
                .AddRedis()
                .AddModuleInfo(modules)
                .AddAuth(modules)
                .AddMemoryCache()
                .AddSingleton<IClock, UtcClock>()
                .AddSingleton<IRequestStorage, RequestStorage>()
                .AddSingleton<IRng, Rng>()
                .AddSingleton<IIdGenerator, IdGenerator>()
                .AddScoped<ErrorHandlerMiddleware>()
                .AddScoped<UserMiddleware>()
                .AddSingleton<IExceptionToResponseMapper, ExceptionToResponseMapper>()
                .AddSingleton<IExceptionToMessageMapper, ExceptionToMessageMapper>()
                .AddSingleton<IExceptionCompositionRoot, ExceptionCompositionRoot>()
                .AddSingleton<IExceptionToMessageMapperResolver, ExceptionToMessageMapperResolver>()
                .AddSingleton<IDispatcher, InMemoryDispatcher>()
                .AddSingleton<IMessageChannel, MessageChannel>()
                .AddSingleton<IContextFactory, ContextFactory>()
                .AddScoped(ctx => ctx.GetRequiredService<IContextFactory>().Create())
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddCors(cors =>
                {
                    cors.AddPolicy(CorsPolicy, x =>
                    {
                        x.WithOrigins("*")
                            .WithMethods("POST", "PUT", "DELETE")
                            .WithHeaders("Content-Type", "Authorization");
                    });
                })
                .AddControllers()
                .ConfigureApplicationPartManager(manager =>
                {
                    var removedParts = new List<ApplicationPart>();
                    foreach (var disabledModule in disabledModules)
                    {
                        var parts = manager.ApplicationParts.Where(x => x.Name.Contains(disabledModule,
                            StringComparison.InvariantCultureIgnoreCase));
                        removedParts.AddRange(parts);
                    }

                    foreach (var part in removedParts)
                    {
                        manager.ApplicationParts.Remove(part);
                    }

                    manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
                })
                .AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    x.SerializerSettings.Converters = new List<JsonConverter>
                    {
                        new StringEnumConverter(new CamelCaseNamingStrategy())
                    };
                });

            services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));
            services.TryDecorate(typeof(IEventHandler<>), typeof(UnitOfWorkEventHandlerDecorator<>));
            services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));
            services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
            services.TryDecorate(typeof(IEventHandler<>), typeof(LoggingEventHandlerDecorator<>));
            
            var appOptions = services.GetOptions<AppOptions>(sectionName);
            services.AddSingleton(appOptions);            
            if (!appOptions.DisplayBanner || string.IsNullOrWhiteSpace(appOptions.Name))
            {
                return services;
            }

            var version = appOptions.DisplayVersion ? $" {appOptions.Version}" : string.Empty;
            Console.WriteLine(FiggleFonts.Doom.Render($"{appOptions.Name}{version}"));

            return services;
        }
        
        public static IApplicationBuilder ValidateContracts(this IApplicationBuilder app)
        {
            var contractRegistry = app.ApplicationServices.GetRequiredService<IContractRegistry>();
            contractRegistry.Validate();
            
            return app;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseCors(CorsPolicy);
            app.UseMiddleware<UserMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            
            return app;
        }

        public static TModel GetOptions<TModel>(this IServiceCollection services, string settingsSectionName)
            where TModel : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            return configuration.GetOptions<TModel>(settingsSectionName);
        }
        
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName)
            where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(sectionName).Bind(model);
            return model;
        }
    }
}