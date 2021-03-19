using System.Runtime.CompilerServices;
using Chronicle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Infrastructure.Modules;

[assembly: InternalsVisibleTo("Trill.Bootstrapper")]
namespace Trill.Modules.Saga
{
    internal class SagaModule : IModule
    {
        public string Name { get; } = "Saga";
        public string Path { get; } = "saga";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddChronicle();
        }

        public void ConfigureMiddleware(IApplicationBuilder app)
        {
        }

        public void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
        }
    }
}