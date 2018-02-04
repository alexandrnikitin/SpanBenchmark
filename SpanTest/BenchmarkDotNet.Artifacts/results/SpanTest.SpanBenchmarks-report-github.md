``` ini

BenchmarkDotNet=v0.10.12, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.192)
Intel Core i7-4600U CPU 2.10GHz (Haswell), 1 CPU, 4 logical cores and 2 physical cores
Frequency=2630627 Hz, Resolution=380.1375 ns, Timer=TSC
.NET Core SDK=2.1.300-preview2-008042
  [Host] : .NET Core 2.1.0-preview2-26130-06 (Framework 4.6.26130.05), 64bit RyuJIT
  Core   : .NET Core 2.1.0-preview2-26130-06 (Framework 4.6.26130.05), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|              Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
|-------------------- |---------:|----------:|----------:|-------:|---------:|
|      ArrayBenchmark | 17.83 ns | 0.2000 ns | 0.1871 ns |   1.00 |     0.00 |
|     UnsafeBenchmark | 10.49 ns | 0.2387 ns | 0.2344 ns |   0.59 |     0.01 |
|       SpanBenchmark | 29.63 ns | 0.3433 ns | 0.3212 ns |   1.66 |     0.02 |
| SpanUnsafeBenchmark | 18.60 ns | 0.2839 ns | 0.2655 ns |   1.04 |     0.02 |
