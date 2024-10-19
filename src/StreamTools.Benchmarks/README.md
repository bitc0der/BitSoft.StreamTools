# Benchmark results

## StringBufferBenchmark
```text
| Method        | Mean      | Error    | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|-------------- |----------:|---------:|---------:|---------:|---------:|---------:|----------:|
| StringBuilder | 130.96 ms | 7.671 ms | 0.420 ms | 500.0000 | 500.0000 | 500.0000 |    256 MB |
| ArrayBuilder  |  30.33 ms | 1.542 ms | 0.085 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
| MemoryBuilder |  30.43 ms | 3.048 ms | 0.167 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
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
| StrinbStream             | 163.1 ms | 42.50 ms | 2.33 ms | 2500.0000 | 2500.0000 | 2500.0000 | 256.05 MB |
| StrinbStream_ArrayPool   | 174.6 ms |  6.17 ms | 0.34 ms |         - |         - |         - |    128 MB |
| StrinbStream_MemoeryPool | 168.3 ms | 17.35 ms | 0.95 ms |         - |         - |         - |    128 MB |
| MemoryStream             | 190.0 ms | 21.20 ms | 1.16 ms | 4666.6667 | 4666.6667 | 4666.6667 | 255.88 MB |
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