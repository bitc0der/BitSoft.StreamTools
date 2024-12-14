# Benchmark results

## StringBufferWriteBenchmark
```text
| Method                  | Mean     | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|------------------------ |---------:|---------:|---------:|---------:|---------:|---------:|----------:|
| StringBuilderBuffer     | 39.31 ms | 7.892 ms | 0.433 ms | 272.7273 | 272.7273 | 272.7273 |    256 MB |
| ArrayStringBuffer       | 30.26 ms | 0.379 ms | 0.021 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
| MemoryStringBuffer      | 30.39 ms | 2.484 ms | 0.136 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
| Encoding_UTF8_GetString | 20.90 ms | 2.323 ms | 0.127 ms | 937.5000 | 937.5000 | 937.5000 |    128 MB |
```

## StringStreamReadBenchmark
```text
| Method            | Mean     | Error     | StdDev   | Allocated  |
|------------------ |---------:|----------:|---------:|-----------:|
| StringStream_Read | 35.90 ms | 25.767 ms | 1.412 ms |  125.91 KB |
| MemoryStream      | 47.87 ms |  5.071 ms | 0.278 ms | 65656.9 KB |
```

## StringStreamWriteBenchmark
```text
| Method                              | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|------------------------------------ |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| StringStream_WriteWithStringBuilder | 166.6 ms | 56.72 ms | 3.11 ms | 2500.0000 | 2500.0000 | 2500.0000 | 256.05 MB |
| StringStream_WriteWithArrayPool     | 175.6 ms | 36.60 ms | 2.01 ms |         - |         - |         - |    128 MB |
| StringStream_WriteWithMemoryPool    | 178.5 ms | 16.09 ms | 0.88 ms |         - |         - |         - |    128 MB |
| MemoryStream                        | 186.2 ms |  8.86 ms | 0.49 ms | 4666.6667 | 4666.6667 | 4666.6667 | 255.88 MB |
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