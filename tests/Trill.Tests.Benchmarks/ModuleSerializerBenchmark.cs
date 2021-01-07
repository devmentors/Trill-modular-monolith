using System;
using BenchmarkDotNet.Attributes;
using Trill.Shared.Infrastructure.Modules;

namespace Trill.Tests.Benchmarks
{
    [HtmlExporter]
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    internal class ModuleSerializerBenchmark
    {
        private readonly IModuleSerializer _jsonSerializer = new JsonModuleSerializer();
        private readonly IModuleSerializer _messagePackSerializer = new MessagePackModuleSerializer();

        private readonly Data _data = new Data
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Amount = 100000,
            CreatedAt = DateTime.UtcNow
        };

        private readonly byte[] _serializedJson;
        private readonly byte[] _serializedMessagePack;

        public ModuleSerializerBenchmark()
        {
            _serializedJson = _jsonSerializer.Serialize(_data);
            _serializedMessagePack = _messagePackSerializer.Serialize(_data);
        }

        [Benchmark]
        public byte[] Json_Serialize() => _jsonSerializer.Serialize(_data);

        [Benchmark]
        public byte[] MessagePack_Serialize() => _messagePackSerializer.Serialize(_data);

        [Benchmark]
        public Data Json_Deserialize() => _jsonSerializer.Deserialize<Data>(_serializedJson);

        [Benchmark]
        public Data MessagePack_Deserialize() => _messagePackSerializer.Deserialize<Data>(_serializedMessagePack);

        internal class Data
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public long Amount { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}