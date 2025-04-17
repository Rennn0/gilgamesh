using __BENCH__.Source;
// using __BENCH__.Source;
using BenchmarkDotNet.Running;

// BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<DbBench>();

BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<DbCursorVsWhileBench>();

// [x]  vs resharper source kods amoyris?  amoyara
