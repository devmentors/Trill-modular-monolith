using System.Net.Http;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Trill.Tests.Performance.Common
{
    internal static class IdentityExtensions
    {
        public static async Task<AuthResult> SignInAsync(this HttpClient httpClient, string name, string password,
            ITestOutputHelper output = null)
        {
            var response = await httpClient.PostAsync(GetEndpoint("sign-in"), new {name, password}, output);

            return await response.ReadAsync<AuthResult>();
        }

        private static string GetEndpoint(string endpoint) => $"users-module/{endpoint}";
    }
}