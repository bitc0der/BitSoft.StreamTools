# Benchmark results

## StringBufferBenchmark
```text
| Method        | Mean     | Error     | StdDev   | Gen0     | Gen1     | Gen2     | Allocated |
|-------------- |---------:|----------:|---------:|---------:|---------:|---------:|----------:|
| StringBuilder | 83.93 ms | 31.192 ms | 1.710 ms | 333.3333 | 333.3333 | 333.3333 |    256 MB |
| ArrayBuilder  | 30.29 ms |  4.084 ms | 0.224 ms | 250.0000 | 250.0000 | 250.0000 |    128 MB |
```

## StringStreamBenchmark
```text
| Method       | Mean     | Error     | StdDev   | Allocated  |
|------------- |---------:|----------:|---------:|-----------:|
| StringStream | 36.01 ms | 12.910 ms | 0.708 ms |   125.9 KB |
| MemoryStream | 46.32 ms |  4.561 ms | 0.250 ms | 65656.9 KB |
```