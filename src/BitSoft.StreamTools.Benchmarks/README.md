# Benchmark results

## StringBufferWriteBenchmark
```text
| Method                     | Mean      | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|--------------------------- |----------:|----------:|---------:|---------:|---------:|---------:|----------:|
| StringBuilderBuffer        | 192.39 ms |  24.47 ms | 1.341 ms |        - |        - |        - |   1200 MB |
| ArrayStringBuffer          | 127.44 ms |  41.02 ms | 2.248 ms |        - |        - |        - |    600 MB |
| MemoryStringBuffer         | 126.40 ms |  16.70 ms | 0.915 ms |        - |        - |        - |    600 MB |
| ArrayPoolQueueStringBuffer |  89.59 ms | 115.36 ms | 6.323 ms |        - |        - |        - |   2.01 MB |
| Encoding_UTF8_GetString    | 102.54 ms |  16.28 ms | 0.893 ms | 600.0000 | 600.0000 | 600.0000 |    600 MB |
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
| StringStream_WriteWithStringBuilder   |   677.9 ms |    21.71 ms |  1.19 ms | 4000.0000 | 4000.0000 | 4000.0000 | 1228988.47 KB |
| StringStream_WriteWithArrayPool       |   862.9 ms |    52.80 ms |  2.89 ms |         - |         - |         - |  614401.23 KB |
| StringStream_WriteWithArrayPoolQueued |   585.8 ms | 1,053.64 ms | 57.75 ms |         - |         - |         - |     257.28 KB |
| StringStream_WriteWithMemoryPool      |   860.9 ms |    78.02 ms |  4.28 ms |         - |         - |         - |  614401.51 KB |
| MemoryStream                          | 1,310.2 ms |    16.97 ms |  0.93 ms | 7000.0000 | 7000.0000 | 7000.0000 | 2097030.05 KB |
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