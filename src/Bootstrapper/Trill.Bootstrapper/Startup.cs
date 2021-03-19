using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Trill.Shared.Infrastructure;
using Trill.Shared.Infrastructure.Modules;

namespace Trill.Bootstrapper
{
    internal class Startup
    {
        private readonly ISet<string> _devEnvironments = new HashSet<string> {"development", "local", "test"};
        private readonly IList<IModule> _modules;
        private readonly IList<Assembly> _assemblies;

        public Startup(IConfiguration configuration)
        {
            _assemblies = ModuleLoader.LoadAssemblies(configuration);
            _modules = ModuleLoader.LoadModules(_assemblies);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(_assemblies, _modules);
            foreach (var module in _modules)
            {
                module.ConfigureServices(services);
            }
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILogger<Startup> logger)
        {
            if (_devEnvironments.Contains(env.EnvironmentName))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseInfrastructure();
            foreach (var module in _modules)
            {
                logger.LogInformation($"Configuring the middleware for: '{module.Name}'...");
                module.ConfigureMiddleware(app);
            }

            app.ValidateContracts();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", ctx => ctx.Response.WriteAsync("Trill API"));
                endpoints.MapModuleInfo();

                foreach (var module in _modules)
                {
                    logger.LogInformation($"Configuring the endpoints for: '{module.Name}', path: '/{module.Path}'...");
                    module.ConfigureEndpoints(endpoints);
                }
            });

            _assemblies.Clear();
            _modules.Clear();
        }
    }
}
