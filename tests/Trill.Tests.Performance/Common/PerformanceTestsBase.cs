using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http;
using NBomber.Plugins.Http.CSharp;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Trill.Tests.Performance.Common
{
    [ExcludeFromCodeCoverage]
    [Collection("sequential")]
    public abstract class PerformanceTestsBase : IDisposable
    {
        protected async Task<NodeStats> RunAuthenticatedScenarioAsync(string name, Func<HttpRequest> request, int parallelCopies = 100,
            TimeSpan? period = null)
        {
            var jwt = await AuthenticateAsync();
            HttpRequest AuthRequest() => request().WithHeader("Authorization", $"Bearer {jwt.AccessToken}");
            return RunScenario(name, AuthRequest, parallelCopies, period);
        }

        protected NodeStats RunScenario(string name, Func<HttpRequest> request, int parallelCopies = 100,
            TimeSpan? period = null)
        {
            var step = HttpStep.Create("init", ctx => Task.FromResult(request()));
            var scenario = ScenarioBuilder.CreateScenario(name, step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(3))
                .WithLoadSimulations(LoadSimulation.NewKeepConstant(parallelCopies, period ?? TimeSpan.FromSeconds(10)));

            var stats = NBomberRunner.RegisterScenarios(scenario).Run();
            var jsonStats = JsonConvert.SerializeObject(stats, Formatting.Indented);
            Output.WriteLine(jsonStats);

            return stats;
        }

        protected async Task<AuthResult> AuthenticateAsync(string name = null, string password = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "test";
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                password = "secret";
            }

            Output.WriteLine($"Authenticating the user: '{name}', password: '{password}'...");
            var jwtDto = await HttpClient.SignInAsync(name, password);
            Output.WriteLine($"Authenticated the user: '{name}', password: '{password}'. " +
                             $"User ID: '{jwtDto.UserId}', role: '{jwtDto.Role}', " +
                             $"access token: '{jwtDto.AccessToken}'");

            return jwtDto;
        }

        protected async Task<AuthResult> AuthenticateAndSetHeaderAsync(string name = null, string password = null)
        {
            const string header = "authorization";
            var jwtDto = await AuthenticateAsync(name, password);
            HttpClient.DefaultRequestHeaders.Remove(header);
            HttpClient.DefaultRequestHeaders.TryAddWithoutValidation(header, $"bearer {jwtDto.AccessToken}");

            return jwtDto;
        }
        
        protected static Task<Response> AssertOkResponse(HttpResponseMessage response)
            => Task.FromResult(response.IsSuccessStatusCode ? Response.Ok() : Response.Fail());

        protected static StringContent GetPayload(object value)
            => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
        
        #region Arrange

        protected readonly HttpClient HttpClient;
        protected readonly ITestOutputHelper Output;
        protected readonly string ApiUrl;

        protected PerformanceTestsBase(ITestOutputHelper output)
        {
            ApiUrl = "http://localhost:5000";
            Output = output;
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(ApiUrl)
            };
        }

        public virtual void Dispose()
        {
            HttpClient.Dispose();
        }

        #endregion
    }
}