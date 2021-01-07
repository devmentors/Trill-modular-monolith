using BenchmarkDotNet.Running;

namespace Trill.Tests.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ModuleSerializerBenchmark>();
            BenchmarkRunner.Run<SerializerBenchmark>();
        }
    }
}