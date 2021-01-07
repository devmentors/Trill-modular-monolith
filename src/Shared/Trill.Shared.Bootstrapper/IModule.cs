using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Trill.Shared.Bootstrapper
{
    public interface IModule
    {
        string Name { get; }
        string Path { get; }
        string Schema { get; }
        void ConfigureServices(IServiceCollection services);
        void ConfigureMiddleware(IApplicationBuilder app);
        void ConfigureEndpoints(IEndpointRouteBuilder endpoints);
    }
}