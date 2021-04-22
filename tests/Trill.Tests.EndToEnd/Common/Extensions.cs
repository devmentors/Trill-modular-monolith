using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Trill.Tests.EndToEnd.Common
{
    internal static class Extensions
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new JsonStringEnumConverter()}
        };

        public static StringContent GetPayload(this object value)
            => new(JsonSerializer.Serialize(value, SerializerOptions), Encoding.UTF8, "application/json");

        public static async Task<T> ReadAsync<T>(this HttpResponseMessage response)
            => JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), SerializerOptions);
    }
}