using System.Data;
using BenchmarkDotNet.Attributes;
using Microsoft.Data.SqlClient;

namespace __BENCH__.Source
{
    [MemoryDiagnoser]
    [RankColumn]
    public class DbCursorVsWhileBench
    {
        private readonly string _connection =
            Environment.GetEnvironmentVariable("DEV_CONNECTION")
            ?? throw new ArgumentNullException("env variable DEV_CONNECTION is null");
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private SqlConnection _sqlConnection;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Params(100, 1000, 10000)]
        public int Count;

        [GlobalSetup]
        public void Setup()
        {
            _sqlConnection = new SqlConnection(_connection);
            _sqlConnection.Open();
        }

        [Benchmark]
        public void WhileLoopBenchmark()
        {
            using var command = new SqlCommand("sp_Benchmark_WhileLoop", _sqlConnection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@RowCount", Count);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                /* Process data if needed */
            }
        }

        [Benchmark]
        public void CursorBenchmark()
        {
            using var command = new SqlCommand("sp_Benchmark_Cursor", _sqlConnection)
            {
                CommandType = CommandType.StoredProcedure,
            };
            command.Parameters.AddWithValue("@RowCount", Count);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                /* Process data if needed */
            }
        }
    }
}
