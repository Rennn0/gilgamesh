```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.5737/22H2/2022Update)
12th Gen Intel Core i7-12700K, 1 CPU, 20 logical and 12 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX2


```
| Method             | Count | Mean      | Error     | StdDev     | Median    | Rank | Allocated |
|------------------- |------ |----------:|----------:|-----------:|----------:|-----:|----------:|
| **WhileLoopBenchmark** | **100**   |  **25.95 ms** |  **1.674 ms** |   **4.777 ms** |  **24.39 ms** |    **1** |   **2.09 KB** |
| CursorBenchmark    | 100   |  67.94 ms |  2.313 ms |   6.370 ms |  66.57 ms |    2 |   2.19 KB |
| **WhileLoopBenchmark** | **1000**  | **152.15 ms** |  **7.163 ms** |  **20.667 ms** | **149.11 ms** |    **3** |   **2.17 KB** |
| CursorBenchmark    | 1000  | 261.94 ms |  8.986 ms |  25.492 ms | 267.92 ms |    4 |   2.48 KB |
| **WhileLoopBenchmark** | **10000** | **724.24 ms** | **61.059 ms** | **180.034 ms** | **721.48 ms** |    **5** |   **2.94 KB** |
| CursorBenchmark    | 10000 | 866.40 ms | 61.498 ms | 179.391 ms | 852.02 ms |    6 |   2.92 KB |
