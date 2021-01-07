using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NBomber.Contracts;
using Newtonsoft.Json;
using Shouldly;
using Xunit.Abstractions;

namespace Trill.Tests.Performance.Common
{
    internal static class Extensions
    {
        public static async Task<T> GetAsync<T>(this HttpClient httpClient, string endpoint,
            ITestOutputHelper output = null)
        {
            var response = await GetAsync(httpClient, endpoint, output);

            return response.IsSuccessStatusCode ? await ReadAsync<T>(response) : default;
        }

        public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, string endpoint,
            ITestOutputHelper output = null)
            => httpClient.SendAsync(HttpMethod.Get, endpoint, output);

        public static Task<HttpResponseMessage> PostAsync<T>(this HttpClient httpClient, string endpoint, T command,
            ITestOutputHelper output = null)
            => httpClient.SendAsync(HttpMethod.Post, endpoint, command, output);

        public static Task<HttpResponseMessage> PutAsync<T>(this HttpClient httpClient, string endpoint, T command,
            ITestOutputHelper output = null)
            => httpClient.SendAsync(HttpMethod.Put, endpoint, command, output);

        public static Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string endpoint,
            ITestOutputHelper output = null)
            => httpClient.SendAsync(HttpMethod.Delete, endpoint, output);

        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpMethod method,
            string endpoint, object data = null, ITestOutputHelper output = null)
        {
            var requestId = Guid.NewGuid().ToString("N");
            var apiEndpoint = $"{httpClient.BaseAddress}/{endpoint}";
            output?.WriteLine($"Sending a request [ID: '{requestId}'] HTTP {method} '{apiEndpoint}'" +
                              $"{(data is {} ? $", body: {JsonConvert.SerializeObject(data, Formatting.Indented)}" : "")}...");
            var response = method.Method switch
            {
                "GET" => await httpClient.GetAsync(endpoint),
                "POST" => await httpClient.PostAsync(endpoint, GetPayload(data)),
                "PUT" => await httpClient.PutAsync(endpoint, GetPayload(data)),
                "DELETE" => await httpClient.DeleteAsync(endpoint),
                _ => throw new InvalidOperationException($"Unsupported HTTP method: {method}")
            };
            output?.WriteLine($"Received a response for request [ID: '{requestId}'] HTTP {method} '{apiEndpoint}'.");
            output?.WriteLine(response.ToString());
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(content))
            {
                output?.WriteLine($"Content:{Environment.NewLine}{content}");
            }

            response.IsSuccessStatusCode.ShouldBeTrue();

            return response;
        }

        private static StringContent GetPayload(object value)
            => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

        public static async Task<T> ReadAsync<T>(this HttpResponseMessage response)
            => JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

        public static Guid GetIdFromLocationHeader(this HttpResponseMessage response)
            => Guid.Parse(response.Headers.Location.ToString().Split("/").Last());

        public static void EnsureCreated(this HttpResponseMessage response)
        {
            response.IsSuccessStatusCode.ShouldBeTrue();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            response.Headers.Location.ShouldNotBeNull();
        }

        public static void Assert(this NodeStats _, Action<NodeStats> stats)
        {
            stats(_);
        }
    }
}