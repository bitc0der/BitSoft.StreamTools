# Benchmark results

## StringBufferWriteBenchmark
```text
| Method                     | Mean     | Error    | StdDev  | Gen0     | Gen1     | Gen2     | Allocated |
|--------------------------- |---------:|---------:|--------:|---------:|---------:|---------:|----------:|
| StringBuilderBuffer        | 189.2 ms | 67.02 ms | 3.67 ms |        - |        - |        - |   1200 MB |
| ArrayStringBuffer          | 127.3 ms | 35.76 ms | 1.96 ms |        - |        - |        - |    600 MB |
| MemoryStringBuffer         | 124.5 ms |  3.03 ms | 0.17 ms |        - |        - |        - |    600 MB |
| ArrayPoolQueueStringBuffer | 104.3 ms | 44.72 ms | 2.45 ms |        - |        - |        - |      2 MB |
| Encoding_UTF8_GetString    | 103.6 ms | 29.39 ms | 1.61 ms | 600.0000 | 600.0000 | 600.0000 |    600 MB |
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
| Method                                | Mean       | Error       | StdDev   | Gen0      | Gen1      | Gen2      | Allocated     |
|-------------------------------------- |-----------:|------------:|---------:|----------:|----------:|----------:|--------------:|
| StringStream_WriteWithStringBuilder   |   698.8 ms |     2.78 ms |  0.15 ms | 4000.0000 | 4000.0000 | 4000.0000 | 1228988.47 KB |
| StringStream_WriteWithArrayPool       |   871.4 ms |   101.02 ms |  5.54 ms |         - |         - |         - |  614401.23 KB |
| StringStream_WriteWithArrayPoolQueued |   581.5 ms | 1,103.79 ms | 60.50 ms |         - |         - |         - |     257.29 KB |
| StringStream_WriteWithMemoryPool      |   866.4 ms |    16.37 ms |  0.90 ms |         - |         - |         - |  614401.51 KB |
| MemoryStream                          | 1,311.4 ms |    38.63 ms |  2.12 ms | 7000.0000 | 7000.0000 | 7000.0000 | 2097030.05 KB |
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