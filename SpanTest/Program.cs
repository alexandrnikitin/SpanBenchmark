using BenchmarkDotNet.Running;
using SpanTest.RealWorldCases;

namespace SpanTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SpanBenchmarks>();
        }
    }
}
