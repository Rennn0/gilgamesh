using __BENCH__.Models;
using __BENCH__.Source;
using BenchmarkDotNet.Running;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<DbBench>();

// [x]  vs resharper source kods amoyris?  amoyara 