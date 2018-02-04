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
|      ArrayBenchmark | 15.46 ns | 0.0847 ns | 0.0792 ns |   1.00 |     0.00 |
|     UnsafeBenchmark | 10.71 ns | 0.1411 ns | 0.1250 ns |   0.69 |     0.01 |
|       SpanBenchmark | 27.84 ns | 0.2352 ns | 0.2200 ns |   1.80 |     0.02 |
| SpanUnsafeBenchmark | 19.39 ns | 0.1233 ns | 0.1153 ns |   1.25 |     0.01 |
