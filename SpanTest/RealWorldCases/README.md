``` ini

BenchmarkDotNet=v0.10.12, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.192)
Intel Core i7-4600U CPU 2.10GHz (Haswell), 1 CPU, 4 logical cores and 2 physical cores
Frequency=2630627 Hz, Resolution=380.1375 ns, Timer=TSC
.NET Core SDK=2.1.300-preview2-008042
  [Host]     : .NET Core 2.1.0-preview2-26130-06 (Framework 4.6.26130.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.0-preview2-26130-06 (Framework 4.6.26130.05), 64bit RyuJIT


```
|                    Method |     Mean |    Error |   StdDev | Scaled | ScaledSD |
|-------------------------- |---------:|---------:|---------:|-------:|---------:|
| DistanceUnsafeShortString | 135.0 ns | 1.332 ns | 1.246 ns |   1.00 |     0.00 |
|   DistanceSpanShortString | 182.1 ns | 1.575 ns | 1.474 ns |   1.35 |     0.02 |
