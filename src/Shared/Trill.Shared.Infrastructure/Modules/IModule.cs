using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Trill.Shared.Infrastructure.Modules
{
    public interface IModule
    {
        string Name { get; }
        string Path { get; }
        IEnumerable<string> Policies => null;
        void ConfigureServices(IServiceCollection services);
        void ConfigureMiddleware(IApplicationBuilder app);
        void ConfigureEndpoints(IEndpointRouteBuilder endpoints);
    }
}