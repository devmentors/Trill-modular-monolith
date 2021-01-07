using System;
using System.Text;
using Newtonsoft.Json;

namespace Trill.Shared.Infrastructure.Modules
{
    internal class JsonModuleSerializer : IModuleSerializer
    {
        public byte[] Serialize<T>(T value)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));

        public T Deserialize<T>(byte[] value)
            => JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value));

        public object Deserialize(byte[] value, Type type)
            => JsonConvert.DeserializeObject(Encoding.UTF8.GetString(value), type);
    }
}