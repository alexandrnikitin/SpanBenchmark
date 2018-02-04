using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;

namespace SpanTest
{
    [CoreJob]
    [DisassemblyDiagnoser(printSource:true)]
    public class SpanBenchmarks
    {
        [Benchmark(Baseline = true)]
        public int ArrayBenchmark()
        {
            var x1 = new int[16];
            var sum = 0;
            for (int i = 0; i < x1.Length; i++)
            {
                sum += x1[i];
            }
            return sum;
        }

        [Benchmark]
        public unsafe int UnsafeBenchmark()
        {
            int* x1 = stackalloc int[16];
            var sum = 0;
            for (int i = 0; i < 16; i++)
            {
                sum += x1[i];
            }
            return sum;
        }

        [Benchmark]
        public int SpanBenchmark()
        {
            Span<int> x1 = stackalloc int[16];
            var sum = 0;
            for (int i = 0; i < x1.Length; i++)
            {
                sum += x1[i];
            }
            return sum;
        }

        [Benchmark]
        public int SpanUnsafeBenchmark()
        {
            Span<int> x1;

            unsafe
            {
                int* tmp1 = stackalloc int[16];
                x1 = new Span<int>(tmp1, 16);
            }

            var sum = 0;
            for (int i = 0; i < x1.Length; i++)
            {
                sum += x1[i];
            }
            return sum;
        }

    }
}