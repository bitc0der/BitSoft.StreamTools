# Benchmark results

## StringBufferWriteBenchmark
```text
| Method                  | Mean     | Error     | StdDev  | Gen0     | Gen1     | Gen2     | Allocated |
|------------------------ |---------:|----------:|--------:|---------:|---------:|---------:|----------:|
| StringBuilderBuffer     | 283.8 ms | 143.34 ms | 7.86 ms | 500.0000 | 500.0000 | 500.0000 |   2224 MB |
| ArrayStringBuffer       | 173.6 ms |  26.92 ms | 1.48 ms |        - |        - |        - |    600 MB |
| MemoryStringBuffer      | 155.0 ms |  80.52 ms | 4.41 ms | 250.0000 | 250.0000 | 250.0000 |    856 MB |
| Encoding_UTF8_GetString | 107.3 ms |  25.91 ms | 1.42 ms | 600.0000 | 600.0000 | 600.0000 |    600 MB |
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
| StringStream_WriteWithStringBuilder |   708.7 ms |  65.18 ms |  3.57 ms | 4000.0000 | 4000.0000 | 4000.0000 | 1200.18 MB |
| StringStream_WriteWithArrayPool     |   844.8 ms | 141.78 ms |  7.77 ms |         - |         - |         - |     600 MB |
| StringStream_WriteWithMemoryPool    |   886.8 ms | 213.93 ms | 11.73 ms |         - |         - |         - |     600 MB |
| MemoryStream                        | 1,316.5 ms |   1.44 ms |  0.08 ms | 7000.0000 | 7000.0000 | 7000.0000 | 2047.88 MB |
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
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.2033)
AMD Ryzen 7 5800U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.403
  [Host]   : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.10 (8.0.1024.46610), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1
WarmupCount=3
```