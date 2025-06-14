using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Core;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Consumer.BackgroundWorkers
{
    public class SearchEngineWorker : BackgroundService
    {
        private readonly string? _connectionString;
        private readonly List<Model> _models;
        private readonly ILogger<SearchEngineWorker> _logger;

        public SearchEngineWorker(IConfiguration configuration, ILogger<SearchEngineWorker> logger)
        {
            _connectionString = configuration.GetConnectionString("gmesh");
            _models = [];
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var sw = Stopwatch.StartNew();
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(stoppingToken);
            const string sql = "select * from MOCK_DATA";
            using SqlCommand command = new SqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync(stoppingToken);
            while (await reader.ReadAsync(stoppingToken))
            {
                _models.Add(new Model
                {
                    CompanyName = (string)reader["company_name"],
                    EmployeeCount = (int)reader["employee_count"],
                    Industry = (string)reader["industry"],
                    Revenue = (decimal)reader["revenue"],
                    Location = (string)reader["location"],
                    ContactPerson = (string)reader["contact_person"],
                    Phone = (string)reader["phone_number"],
                    Founded = (DateTime)reader["founded_date"],
                    ProfitMargin = (decimal)reader["profit_margin"],
                });
            }
            sw.Stop();
            long bytes = Encoding.UTF8.GetByteCount(JsonSerializer.Serialize(_models));
            float inKbs = bytes / 1024;
            _logger.LogCritical($"ESTIMATED SIZE {inKbs} kbs, {_models.Count}, {sw.ElapsedMilliseconds} ms");

            System.Collections.ObjectModel.ReadOnlyCollection<Model> l1 = _models.Where(x => x.CompanyName is null || x.CompanyName.Contains("Ed")).ToList().AsReadOnly();

            string s = JsonSerializer.Serialize(l1);
            bytes = Encoding.UTF8.GetByteCount(s);
            inKbs = bytes / 1024;
            _logger.LogCritical($"ESTIMATED SIZE {inKbs} kbs, {l1.Count}, {s.Length}");
        }

        private class Model
        {
            internal string? CompanyName { get; set; }
            internal int EmployeeCount { get; set; }
            internal string? Industry { get; set; }
            internal decimal Revenue { get; set; }
            internal string? Location { get; set; }
            internal string? ContactPerson { get; set; }
            internal string? Phone { get; set; }
            internal DateTime Founded { get; set; }
            internal decimal ProfitMargin { get; set; }
        }
    }
}