using System.Collections.Generic;

namespace Trill.Shared.Abstractions.Auth
{
    public class JsonWebTokenPayload
    {
        public string Subject { get; set; }
        public string Role { get; set; }
        public long Expires { get; set; }
        public IDictionary<string, IEnumerable<string>> Claims { get; set; }
    }
}