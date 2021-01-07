using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Trill.Tests.EndToEnd.Common
{
    internal static class Extensions
    {
        public static StringContent GetPayload(this object value)
            => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

        public static async Task<T> ReadAsync<T>(this HttpResponseMessage response)
            => JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
    }
}