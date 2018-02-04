# Span<> tests
A repro for coreclr issue


### Benchmark results

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


### Assembly code:

The assembly code Span generates:

```asm

00007ffc`fe39eaf0 SpanTest.SpanBenchmarks.SpanBenchmark()	
            Span x1 = stackalloc int[16];
            ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
00007ffc`fe39eb22 b804000000      mov     eax,4
00007ffc`fe39eb27 6a00            push    0
00007ffc`fe39eb29 6a00            push    0
00007ffc`fe39eb2b 48ffc8          dec     rax
00007ffc`fe39eb2e 75f7            jne     00007ffc`fe39eb27
00007ffc`fe39eb30 488d0424        lea     rax,[rsp]
00007ffc`fe39eb34 48896538        mov     qword ptr [rbp+38h],rsp
00007ffc`fe39eb38 33d2            xor     edx,edx
00007ffc`fe39eb3a 48895510        mov     qword ptr [rbp+10h],rdx
00007ffc`fe39eb3e 48894510        mov     qword ptr [rbp+10h],rax
00007ffc`fe39eb42 488b4510        mov     rax,qword ptr [rbp+10h]
00007ffc`fe39eb46 488d5518        lea     rdx,[rbp+18h]
00007ffc`fe39eb4a 488902          mov     qword ptr [rdx],rax
00007ffc`fe39eb4d c7452010000000  mov     dword ptr [rbp+20h],10h
            var sum = 0;
            ^^^^^^^^^^^^
00007ffc`fe39eb54 c4e17a6f4518    vmovdqu xmm0,xmmword ptr [rbp+18h]
00007ffc`fe39eb5a c4e17a7f4528    vmovdqu xmmword ptr [rbp+28h],xmm0
00007ffc`fe39eb60 33c0            xor     eax,eax
            for (int i = 0; i < x1.Length; i++)
                 ^^^^^^^^^
00007ffc`fe39eb62 33d2            xor     edx,edx
00007ffc`fe39eb64 8b4d30          mov     ecx,dword ptr [rbp+30h]
00007ffc`fe39eb67 85c9            test    ecx,ecx
00007ffc`fe39eb69 7e11            jle     00007ffc`fe39eb7c
                sum += x1[i];
                ^^^^^^^^^^^^^
00007ffc`fe39eb6b 4c8b4528        mov     r8,qword ptr [rbp+28h]
00007ffc`fe39eb6f 4c63ca          movsxd  r9,edx
00007ffc`fe39eb72 43030488        add     eax,dword ptr [r8+r9*4]
            for (int i = 0; i < x1.Length; i++)
                                           ^^^
00007ffc`fe39eb76 ffc2            inc     edx
00007ffc`fe39eb78 3bd1            cmp     edx,ecx
00007ffc`fe39eb7a 7cef            jl      00007ffc`fe39eb6b
            return sum;
            ^^^^^^^^^^^
00007ffc`fe39eb7c 48b9c368fccd3f200000 mov rcx,203FCDFC68C3h
 ```

Compared to unsafe stack allocation:

```asm


00007ffc`fe3aeb50 SpanTest.SpanBenchmarks.UnsafeBenchmark()	
            int* x1 = stackalloc int[16];
            ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
00007ffc`fe3aeb74 b804000000      mov     eax,4
00007ffc`fe3aeb79 6a00            push    0
00007ffc`fe3aeb7b 6a00            push    0
00007ffc`fe3aeb7d 48ffc8          dec     rax
00007ffc`fe3aeb80 75f7            jne     00007ffc`fe3aeb79
00007ffc`fe3aeb82 488d0424        lea     rax,[rsp]
00007ffc`fe3aeb86 48896508        mov     qword ptr [rbp+8],rsp
            var sum = 0;
            ^^^^^^^^^^^^
00007ffc`fe3aeb8a 33d2            xor     edx,edx
            for (int i = 0; i < 16; i++)
                 ^^^^^^^^^
00007ffc`fe3aeb8c 33c9            xor     ecx,ecx
                sum += x1[i];
                ^^^^^^^^^^^^^
00007ffc`fe3aeb8e 4c63c1          movsxd  r8,ecx
00007ffc`fe3aeb91 42031480        add     edx,dword ptr [rax+r8*4]
            for (int i = 0; i < 16; i++)
                                    ^^^
00007ffc`fe3aeb95 ffc1            inc     ecx
            for (int i = 0; i < 16; i++)
                            ^^^^^^
00007ffc`fe3aeb97 83f910          cmp     ecx,10h
00007ffc`fe3aeb9a 7cf2            jl      00007ffc`fe3aeb8e
            return sum;
            ^^^^^^^^^^^
00007ffc`fe3aeb9c 8bc2            mov     eax,edx

```
