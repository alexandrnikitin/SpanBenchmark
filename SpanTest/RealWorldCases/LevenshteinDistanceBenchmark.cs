using BenchmarkDotNet.Attributes;

namespace SpanTest.RealWorldCases
{
    [DisassemblyDiagnoser(printSource: true)]
    public class LevenshteinDistanceBenchmarks
    {
        [Benchmark(Baseline = true)]
        public int DistanceUnsafeShortString()
        {
            return LevenshteinDistance.DistanceUnsafe("kitten", "sitting");
        }

        [Benchmark]
        public int DistanceSpanShortString()
        {
            return LevenshteinDistance.DistanceSpan("kitten", "sitting");
        }
    }
}