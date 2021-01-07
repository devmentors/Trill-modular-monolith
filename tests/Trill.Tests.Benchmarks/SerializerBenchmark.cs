using System;
using BenchmarkDotNet.Attributes;
using MessagePack;
using Newtonsoft.Json;

namespace Trill.Tests.Benchmarks
{
    [HtmlExporter]
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    internal class SerializerBenchmark
    {
        private readonly MessagePackSerializerOptions _messagePackSerializerOptions =
            MessagePack.Resolvers.ContractlessStandardResolver.Options;

        private readonly Data _data = new Data
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Amount = 100000,
            CreatedAt = DateTime.UtcNow
        };

        private readonly string _serializedNewtonsoftJson;
        private readonly string _serializedSystemTextJson;
        private readonly byte[] _serializedMessagePack;

        public SerializerBenchmark()
        {
            _serializedNewtonsoftJson = JsonConvert.SerializeObject(_data);
            _serializedSystemTextJson = System.Text.Json.JsonSerializer.Serialize(_data);
            _serializedMessagePack = MessagePackSerializer.Serialize(_data, _messagePackSerializerOptions);
        }

        [Benchmark]
        public string NewtonsoftJson_Serialize() => JsonConvert.SerializeObject(_data);

        [Benchmark]
        public string SystemTextJson_Serialize() => System.Text.Json.JsonSerializer.Serialize(_data);

        [Benchmark]
        public byte[] MessagePack_Serialize() => MessagePackSerializer.Serialize(_data, _messagePackSerializerOptions);

        [Benchmark]
        public Data NewtonsoftJson_Deserialize() => JsonConvert.DeserializeObject<Data>(_serializedNewtonsoftJson);

        [Benchmark]
        public Data SystemTextJson_Deserialize()
            => System.Text.Json.JsonSerializer.Deserialize<Data>(_serializedSystemTextJson);

        [Benchmark]
        public Data MessagePack_Deserialize()
            => MessagePackSerializer.Deserialize<Data>(_serializedMessagePack, _messagePackSerializerOptions);

        internal class Data
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public long Amount { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}