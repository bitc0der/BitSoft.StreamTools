# Benchmark results

## StringBufferWriteBenchmark
```text
| Method                     | Mean     | Error   | StdDev  | Gen0     | Gen1     | Gen2     | Allocated |
|--------------------------- |---------:|--------:|--------:|---------:|---------:|---------:|----------:|
| StringBuilderBuffer        | 193.6 ms | 6.91 ms | 4.57 ms |        - |        - |        - |   1200 MB |
| ArrayStringBuffer          | 129.2 ms | 5.14 ms | 3.40 ms |        - |        - |        - |    600 MB |
| MemoryStringBuffer         | 127.4 ms | 2.23 ms | 1.33 ms |        - |        - |        - |    600 MB |
| ArrayPoolQueueStringBuffer | 165.0 ms | 2.20 ms | 1.46 ms |        - |        - |        - |    600 MB |
| Encoding_UTF8_GetString    | 106.3 ms | 5.91 ms | 3.91 ms | 600.0000 | 600.0000 | 600.0000 |    600 MB |
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
| Method                                | Mean     | Error      | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|-------------------------------------- |---------:|-----------:|---------:|----------:|----------:|----------:|----------:|
| StringStream_WriteWithStringBuilder   | 641.4 ms |   568.4 ms | 31.16 ms |         - |         - |         - |   3.37 GB |
| StringStream_WriteWithArrayPool       | 538.5 ms | 1,441.9 ms | 79.03 ms | 1000.0000 | 1000.0000 | 1000.0000 |   4.69 GB |
| StringStream_WriteWithArrayPoolQueued | 635.3 ms |   445.6 ms | 24.42 ms |         - |         - |         - |   1.69 GB |
| StringStream_WriteWithMemoryPool      | 506.0 ms |   329.0 ms | 18.03 ms | 1000.0000 | 1000.0000 | 1000.0000 |   4.69 GB |
| MemoryStream                          | 388.3 ms |   645.7 ms | 35.39 ms |         - |         - |         - |   2.53 GB |
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