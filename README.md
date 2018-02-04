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
|      ArrayBenchmark | 17.83 ns | 0.2000 ns | 0.1871 ns |   1.00 |     0.00 |
|     UnsafeBenchmark | 10.49 ns | 0.2387 ns | 0.2344 ns |   0.59 |     0.01 |
|       SpanBenchmark | 29.63 ns | 0.3433 ns | 0.3212 ns |   1.66 |     0.02 |
| SpanUnsafeBenchmark | 18.60 ns | 0.2839 ns | 0.2655 ns |   1.04 |     0.02 |


### Assembly code:

For stack allocated Span:

```asm

00007ffc`fe3aeb50 SpanTest.SpanBenchmarks.SpanBenchmark() |  
-- | --
Span x1 = stackalloc int[16];             
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ |  
00007ffc`fe3aeb84 4883c420        add     rsp,20h |  
00007ffc`fe3aeb88 b804000000      mov     eax,4 |  
00007ffc`fe3aeb8d 6a00            push    0 |  
00007ffc`fe3aeb8f 6a00            push    0 |  
00007ffc`fe3aeb91 48ffc8          dec     rax |  
00007ffc`fe3aeb94 75f7            jne     00007ffc`fe3aeb8d |  
00007ffc`fe3aeb96 4883ec20        sub     rsp,20h |  
00007ffc`fe3aeb9a 488d442420      lea     rax,[rsp+20h] |  
00007ffc`fe3aeb9f 48896538        mov     qword ptr [rbp+38h],rsp |  
00007ffc`fe3aeba3 33d2            xor     edx,edx |  
00007ffc`fe3aeba5 48895510        mov     qword ptr [rbp+10h],rdx |  
00007ffc`fe3aeba9 48894510        mov     qword ptr [rbp+10h],rax |  
00007ffc`fe3aebad 488b4510        mov     rax,qword ptr [rbp+10h] |  
00007ffc`fe3aebb1 488d5518        lea     rdx,[rbp+18h] |  
00007ffc`fe3aebb5 488902          mov     qword ptr [rdx],rax |  
00007ffc`fe3aebb8 c7452010000000  mov     dword ptr [rbp+20h],10h |  
var sum = 0;             
^^^^^^^^^^^^ |  
00007ffc`fe3aebbf c4e17a6f4518    vmovdqu xmm0,xmmword ptr [rbp+18h] |  
00007ffc`fe3aebc5 c4e17a7f4528    vmovdqu xmmword ptr [rbp+28h],xmm0 |  
00007ffc`fe3aebcb 33c0            xor     eax,eax |  
for (int i = 0; i < 16; i++)                  
^^^^^^^^^ |  
00007ffc`fe3aebcd 33d2            xor     edx,edx |  
sum += x1[i];                 
^^^^^^^^^^^^^ |  
00007ffc`fe3aebcf 3b5530          cmp     edx,dword ptr [rbp+30h] |  
00007ffc`fe3aebd2 7330            jae     00007ffc`fe3aec04 |  
00007ffc`fe3aebd4 488b4d28        mov     rcx,qword ptr [rbp+28h] |  
00007ffc`fe3aebd8 4c63c2          movsxd  r8,edx |  
00007ffc`fe3aebdb 42030481        add     eax,dword ptr [rcx+r8*4] |  
for (int i = 0; i < 16; i++)                                     
^^^ |  
00007ffc`fe3aebdf ffc2            inc     edx |  
for (int i = 0; i < 16; i++)                             
^^^^^^ |  
00007ffc`fe3aebe1 83fa10          cmp     edx,10h |  
00007ffc`fe3aebe4 7ce9            jl      00007ffc`fe3aebcf |  
return sum;             
^^^^^^^^^^^ |  
00007ffc`fe3aebe6 48b96464223ccd0c0000 mov rcx,0CCD3C226464h
```

Compared to unsafe stack allocation:

```asm

00007ffc`fe3aeb50 SpanTest.SpanBenchmarks.UnsafeBenchmark() |  
-- | --
int* x1 = stackalloc int[16];             
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ |  
00007ffc`fe3aeb74 b804000000      mov     eax,4 |  
00007ffc`fe3aeb79 6a00            push    0 |  
00007ffc`fe3aeb7b 6a00            push    0 |  
00007ffc`fe3aeb7d 48ffc8          dec     rax |  
00007ffc`fe3aeb80 75f7            jne     00007ffc`fe3aeb79 |  
00007ffc`fe3aeb82 488d0424        lea     rax,[rsp] |  
00007ffc`fe3aeb86 48896508        mov     qword ptr [rbp+8],rsp |  
var sum = 0;             
^^^^^^^^^^^^ |  
00007ffc`fe3aeb8a 33d2            xor     edx,edx |  
for (int i = 0; i < 16; i++)                  
^^^^^^^^^ |  
00007ffc`fe3aeb8c 33c9            xor     ecx,ecx |  
sum += x1[i];                 
^^^^^^^^^^^^^ |  
00007ffc`fe3aeb8e 4c63c1          movsxd  r8,ecx |  
00007ffc`fe3aeb91 42031480        add     edx,dword ptr [rax+r8*4] |  
for (int i = 0; i < 16; i++)                                     
^^^ |  
00007ffc`fe3aeb95 ffc1            inc     ecx |  
for (int i = 0; i < 16; i++)                             
^^^^^^ |  
00007ffc`fe3aeb97 83f910          cmp     ecx,10h |  
00007ffc`fe3aeb9a 7cf2            jl      00007ffc`fe3aeb8e |  
return sum;             
^^^^^^^^^^^ |  
00007ffc`fe3aeb9c 8bc2            mov     eax,edx |  
```