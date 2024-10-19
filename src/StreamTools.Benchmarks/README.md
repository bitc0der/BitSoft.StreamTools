# Benchmark results

## StringBufferBenchmark
```text
| Method              | Mean      | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|-------------------- |----------:|----------:|---------:|---------:|---------:|---------:|----------:|
| StringBuilderBuffer | 121.30 ms |  3.376 ms | 0.185 ms | 500.0000 | 500.0000 | 500.0000 |    256 MB |
| ArrayStringBuffer   |  31.71 ms | 13.252 ms | 0.726 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
| MemoryStringBuffer  |  31.19 ms | 10.741 ms | 0.589 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
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
| Method                   | Mean     | Error    | StdDev  | Gen0      | Gen1      | Gen2      | Allocated |
|------------------------- |---------:|---------:|--------:|----------:|----------:|----------:|----------:|
| StringStream             | 162.0 ms | 16.01 ms | 0.88 ms | 2500.0000 | 2500.0000 | 2500.0000 | 256.05 MB |
| StringStream_ArrayPool   | 178.2 ms | 13.73 ms | 0.75 ms |         - |         - |         - |    128 MB |
| StringStream_MemoeryPool | 169.0 ms |  8.45 ms | 0.46 ms |         - |         - |         - |    128 MB |
| MemoryStream             | 189.7 ms |  6.51 ms | 0.36 ms | 4666.6667 | 4666.6667 | 4666.6667 | 255.88 MB |
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