# Benchmark results

## StringBufferBenchmark
```text
| Method        | Mean     | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|-------------- |---------:|----------:|---------:|---------:|---------:|---------:|----------:|
| StringBuilder | 45.01 ms | 18.671 ms | 1.023 ms | 200.0000 | 200.0000 | 200.0000 |    256 MB |
| ArrayBuilder  | 29.96 ms |  3.201 ms | 0.175 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
| MemoryBuilder | 30.14 ms |  1.585 ms | 0.087 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |```
```

## StringStreamBenchmark
```text
| Method       | Mean     | Error     | StdDev   | Allocated  |
|------------- |---------:|----------:|---------:|-----------:|
| StringStream | 36.01 ms | 12.910 ms | 0.708 ms |   125.9 KB |
| MemoryStream | 46.32 ms |  4.561 ms | 0.250 ms | 65656.9 KB |
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