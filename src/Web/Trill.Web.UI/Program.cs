using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Trill.Web.Core;
using Trill.Web.UI.Services;

namespace Trill.Web.UI
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddCore();
            builder.Services.AddScoped<IApiResponseHandler, ApiResponseHandler>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri("http://localhost:5000")});
            builder.Services.AddAntDesign();
            var host = builder.Build();
            var authenticationService = host.Services.GetRequiredService<IAuthenticationService>();
            await authenticationService.InitializeAsync();
            await host.RunAsync();
        }
    }
}
