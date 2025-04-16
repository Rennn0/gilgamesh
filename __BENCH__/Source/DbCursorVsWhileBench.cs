using System.Data;
using BenchmarkDotNet.Attributes;
using Microsoft.Data.SqlClient;

namespace __BENCH__.Source
{
    [MemoryDiagnoser]
    [RankColumn]
    public class DbCursorVsWhileBench
    {
        readonly string Connection = Environment.GetEnvironmentVariable("DEV_CONNECTION") ?? throw new ArgumentNullException("env variable DEV_CONNECTION is null");
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        SqlConnection sqlConnection;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Params(100, 1000, 10000)]
        public int Count;

        [GlobalSetup]
        public void Setup()
        {
            sqlConnection = new SqlConnection(Connection);
            sqlConnection.Open();
        }


        [Benchmark]
        public void WhileLoopBenchmark()
        {
            using var command = new SqlCommand("sp_Benchmark_WhileLoop", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@RowCount", Count);
            using var reader = command.ExecuteReader();
            while (reader.Read()) { /* Process data if needed */ }
        }

        [Benchmark]
        public void CursorBenchmark()
        {
            using var command = new SqlCommand("sp_Benchmark_Cursor", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@RowCount", Count);
            using var reader = command.ExecuteReader();
            while (reader.Read()) { /* Process data if needed */ }
        }

    }
}