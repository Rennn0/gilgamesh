using System.Text;
using System.Threading.Tasks;
using __BENCH__.Models;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace __BENCH__.Source
{
    [MemoryDiagnoser, ThreadingDiagnoser]
    // [ShortRunJob]
    [MarkdownExporter]
    public class DbBench
    {
        [Params(100, 1000, 10000)]
        public int Count;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private SqlConnection _dapperConnection;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private PotentialClientsDbContext _dbContext;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        readonly string Connection = Environment.GetEnvironmentVariable("DEV_CONNECTION") ?? throw new ArgumentNullException("env variable DEV_CONNECTION is null");

        [GlobalSetup]
        public async Task Setup()
        {
            _dapperConnection = new SqlConnection(Connection);
            await _dapperConnection.OpenAsync();

            _dbContext = new PotentialClientsDbContext(new DbContextOptionsBuilder<PotentialClientsDbContext>()
                .UseSqlServer(Connection, options =>
                {
                    options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                    options.CommandTimeout(300);
                })
                .Options);
            await _dbContext.Organizations.FirstOrDefaultAsync();
        }

        [GlobalCleanup]
        public async Task Cleanup()
        {
            await _dapperConnection.DisposeAsync();
            await _dbContext.DisposeAsync();
        }

        [Benchmark(Baseline = true)]
        public async Task<List<OrganizationModel>> Procedure()
        {
            var foo = await _dapperConnection.QueryAsync<OrganizationModel>("GetOrganizations", new { Scope = "All", Limit = Count, Offset = 0, UserId = 301009 }, commandType: System.Data.CommandType.StoredProcedure);
            return foo.ToList();
        }

        [Benchmark]
        public async Task<List<OrganizationModel>> ProcedureShrinked()
        {
            var foo = await _dapperConnection.QueryAsync<OrganizationModel>("GetOrganizationsTemp", new { Count }, commandType: System.Data.CommandType.StoredProcedure);
            return foo.ToList();
        }

        [Benchmark]
        public async Task<List<OrganizationModel>> RawSql()
        {
            const string sql = @"
SELECT TOP(@Count)
    [o].[OrganizationId],
    [o].[IdentificationCode],
    [o].[OrganizationName],
    [u].[DisplayName] AS [ManagerDisplayName],
    [ns].[Name] AS [NegotiationStatus],
    [pt].[Name] AS [ProductType],
    [p].[Name] AS [Product],
    ISNULL((
        SELECT TOP(1) [c].[CommentText]
        FROM [OrganizationComment] [oc]
        JOIN [dbo].[Comment] [c] ON [c].[CommentId] = [oc].[CommentId]
        WHERE [oc].[OrganizationId] = [o].[OrganizationId]
    ), '') AS [Comment],
    [o].[EmployeeQuantity],
    [o].[Premium],
    [o].[CreatedAt],
    [o].[ContactedAt]
FROM [Organization] [o]
JOIN [Cls_NegotiationStatus] [ns] ON [o].[NegotiationStatusId] = [ns].[NegotiationStatusId]
JOIN [OrganizationProductType] [opt] ON [opt].[OrganizationId] = [o].[OrganizationId]
JOIN [Cls_ProductType] [pt] ON [opt].[ProductTypeId] = [pt].[ProductTypeId]
JOIN [Cls_Product] [p] ON [pt].[ProductId] = [p].[ProductId]
JOIN [syn_AuthDbUsers] [u] ON [u].[UserId] = [o].[ManagerId]";

            var foo = await _dapperConnection.QueryAsync<OrganizationModel>(sql, new { Count });
            return foo.ToList();
        }

        [Benchmark]
        public async Task<List<OrganizationModel>> EfCore()
        {
            // await _dbContext.Organizations.AsNoTracking().Take(Count).Select(o => new OrganizationModel
            // {
            //     IdentificationCode = o.IdentificationCode,
            //     OrganizationId = o.OrganizationId,
            //     OrganizationName = o.OrganizationName,
            //     EmployeeQuantity = o.EmployeeQuantity ?? 0,
            //     ContactedAt = o.ContactedAt.HasValue ? o.ContactedAt.Value : DateTime.Now,
            //     CreatedAt = o.CreatedAt.HasValue ? o.CreatedAt.Value : DateTime.Now,
            //     End = o.EndDate,
            //     ManagerDisplayName = "PIZDEEEEEC",
            //     Premium = o.Premium ?? 0,
            //     NegotiationStatus = o.NegotiationStatus.Name ?? string.Empty,
            //     Comment = o.Comments.Select(c => c.CommentText).FirstOrDefault(),
            //     ProductType = o.ProductTypes.Select(pt => pt.Name).FirstOrDefault() ?? string.Empty,
            //     Product = o.ProductTypes.Select(pt => pt.Product.Name).FirstOrDefault() ?? string.Empty,
            //     Start = o.StartDate,
            // }).ToListAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var foo = await _dbContext.Organizations
    .AsNoTracking()
    .OrderBy(o => o.OrganizationId) // always better to enforce order for perf
    .Take(Count)
    .Select(o => new
    {
        o.OrganizationId,
        o.IdentificationCode,
        o.OrganizationName,
        o.EmployeeQuantity,
        o.ContactedAt,
        o.CreatedAt,
        o.EndDate,
        o.StartDate,
        o.Premium,
        NegotiationStatusName = o.NegotiationStatus.Name,
        Comment = o.Comments.OrderBy(c => c.CommentId).Select(c => c.CommentText).FirstOrDefault(),
        ProductTypeName = o.ProductTypes.OrderBy(pt => pt.ProductTypeId).Select(pt => pt.Name).FirstOrDefault(),
        ProductName = o.ProductTypes.OrderBy(pt => pt.ProductTypeId).Select(pt => pt.Product.Name).FirstOrDefault()
    })
    .Select(o => new OrganizationModel
    {
        OrganizationId = o.OrganizationId,
        IdentificationCode = o.IdentificationCode,
        OrganizationName = o.OrganizationName,
        EmployeeQuantity = o.EmployeeQuantity ?? 0,
        ContactedAt = o.ContactedAt ?? DateTime.Now,
        CreatedAt = o.CreatedAt ?? DateTime.Now,
        End = o.EndDate,
        Start = o.StartDate,
        Premium = o.Premium ?? 0,
        NegotiationStatus = o.NegotiationStatusName ?? string.Empty,
        Comment = o.Comment,
        ProductType = o.ProductTypeName ?? string.Empty,
        Product = o.ProductName ?? string.Empty,
        ManagerDisplayName = "PIZDEEEEEC"
    })
    .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return foo;
        }
    }

    public struct AvailableOrganizationStruct
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string IdentificationCode { get; set; }
    }

    public class OrganizationModel
    {
        public int TotalCount { get; set; }
        public int OrganizationId { get; set; }
        public int EmployeeQuantity { get; set; }
        public decimal Premium { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string IdentificationCode { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string ManagerDisplayName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string OrganizationName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string NegotiationStatus { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string Product { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string ProductType { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public string? Comment { get; set; }
        public DateTime ContactedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }

}
