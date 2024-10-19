# Benchmark results

## StringBufferBenchmark
```text
| Method              | Mean     | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|-------------------- |---------:|----------:|---------:|---------:|---------:|---------:|----------:|
| StringBuilderBuffer | 47.68 ms | 18.388 ms | 1.008 ms | 222.2222 | 222.2222 | 222.2222 |    256 MB |
| ArrayStringBuffer   | 31.31 ms |  7.243 ms | 0.397 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
| MemoryStringBuffer  | 30.67 ms |  0.262 ms | 0.014 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
| MemoryStream        | 21.12 ms |  3.013 ms | 0.165 ms | 937.5000 | 937.5000 | 937.5000 |    128 MB |
```

## StringStreamReadBenchmark
```text
| Method       | Mean     | Error     | StdDev   | Allocated  |
|------------- |---------:|----------:|---------:|-----------:|
| StringStream | 36.01 ms | 12.910 ms | 0.708 ms |   125.9 KB |
| MemoryStream | 46.32 ms |  4.561 ms | 0.250 ms | 65656.9 KB |
```

## StringStreamWriteBenchmark
```text
| Method                         | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|------------------------------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| StringStream_WithStringBuilder | 163.8 ms | 28.43 ms | 1.56 ms | 2500.0000 | 2500.0000 | 2500.0000 | 256.05 MB |
| StringStream_WithArrayPool     | 172.4 ms |  8.27 ms | 0.45 ms |         - |         - |         - |    128 MB |
| StringStream_WithMemoryPool    | 169.0 ms | 10.26 ms | 0.56 ms |         - |         - |         - |    128 MB |
| MemoryStream                   | 190.2 ms | 44.56 ms | 2.44 ms | 4666.6667 | 4666.6667 | 4666.6667 | 255.88 MB |
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