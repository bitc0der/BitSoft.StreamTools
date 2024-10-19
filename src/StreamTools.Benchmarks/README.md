# Benchmark results

## StringBufferBenchmark
```text
| Method        | Mean     | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|-------------- |---------:|----------:|---------:|---------:|---------:|---------:|----------:|
| StringBuilder | 49.59 ms | 36.504 ms | 2.001 ms | 222.2222 | 222.2222 | 222.2222 |    256 MB |
| ArrayBuilder  | 31.25 ms |  5.725 ms | 0.314 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
| MemoryBuilder | 30.68 ms |  6.384 ms | 0.350 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
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
| StrinbStream             | 139.7 ms | 10.96 ms | 0.60 ms |         - |         - |         - |    128 MB |
| StrinbStream_ArrayPool   | 169.1 ms | 19.26 ms | 1.06 ms |         - |         - |         - |    128 MB |
| StrinbStream_MemoeryPool | 168.7 ms | 18.11 ms | 0.99 ms |         - |         - |         - |    128 MB |
| MemoryStream             | 188.5 ms |  9.74 ms | 0.53 ms | 4666.6667 | 4666.6667 | 4666.6667 | 255.88 MB |
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