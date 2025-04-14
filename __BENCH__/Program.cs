using __BENCH__.Source;
using BenchmarkDotNet.Running;

BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<StringManipulation>();

// TODO : efcore vs dapper vs raw sql