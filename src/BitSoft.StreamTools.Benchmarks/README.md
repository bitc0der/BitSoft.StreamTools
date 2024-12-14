# Benchmark results

## StringBufferWriteBenchmark
```text
| Method                  | Mean     | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------------------ |---------:|----------:|---------:|---------:|---------:|---------:|----------:|
| StringBuilderBuffer     | 229.2 ms | 240.43 ms | 13.18 ms |        - |        - |        - |   1200 MB |
| ArrayStringBuffer       | 140.1 ms |  28.09 ms |  1.54 ms |        - |        - |        - |    600 MB |
| MemoryStringBuffer      | 155.0 ms | 158.58 ms |  8.69 ms |        - |        - |        - |    600 MB |
| Encoding_UTF8_GetString | 108.5 ms |  32.01 ms |  1.75 ms | 600.0000 | 600.0000 | 600.0000 |    600 MB |
```

## StringStreamReadBenchmark
```text
| Method            | Mean     | Error     | StdDev  | Allocated |
|------------------ |---------:|----------:|--------:|----------:|
| StringStream_Read | 177.6 ms | 169.89 ms | 9.31 ms |      1 MB |
| MemoryStream      | 226.6 ms |  21.09 ms | 1.16 ms | 300.99 MB |
```

## StringStreamWriteBenchmark
```text
| Method                              | Mean       | Error     | StdDev   | Gen0      | Gen1      | Gen2      | Allocated  |
|------------------------------------ |-----------:|----------:|---------:|----------:|----------:|----------:|-----------:|
| StringStream_WriteWithStringBuilder |   716.7 ms | 109.42 ms |  6.00 ms | 4000.0000 | 4000.0000 | 4000.0000 | 1200.18 MB |
| StringStream_WriteWithArrayPool     |   874.9 ms | 133.81 ms |  7.33 ms |         - |         - |         - |     600 MB |
| StringStream_WriteWithMemoryPool    |   873.6 ms | 186.98 ms | 10.25 ms |         - |         - |         - |     600 MB |
| MemoryStream                        | 1,313.5 ms |  59.93 ms |  3.28 ms | 7000.0000 | 7000.0000 | 7000.0000 | 2047.88 MB |
```

# Legend
```text
  Mean      : Arithmetic mean of all measurements
  Error     : Half of 99.9% confidence interval
  StdDev    : Standard deviation of all measurements
  Gen0      : GC Generation 0 collects per 1000 operations
  Gen1      : GC Generation 1 collects per 1000 operations
  Gen2      : GC Generation 2 collects per 1000 operations
  Allocated : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 ms      : 1 Millisecond (0.001 sec)
```

# System info
```text
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2314)
AMD Ryzen 7 5800U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.101
  [Host]   : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1
WarmupCount=3
```