using System.Runtime.CompilerServices;
using Chronicle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Trill.Shared.Bootstrapper;

[assembly: InternalsVisibleTo("Trill.Api")]
namespace Trill.Modules.Saga
{
    internal class SagaModule : IModule
    {
        public string Name { get; } = "Saga";
        public string Path { get; } = string.Empty;
        public string Schema { get; } = string.Empty;

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