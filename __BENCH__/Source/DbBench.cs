using System.Text;
using System.Threading.Tasks;
using __BENCH__.Models;
using BenchmarkDotNet.Attributes;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace __BENCH__.Source
{
    [MemoryDiagnoser]
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
        const string Connection = "";

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
        public async Task Procedure()
        {
            await _dapperConnection.QueryAsync<AvailableOrganizationStruct>("GetOrganizations", new { Scope = "All", Limit = Count, Offset = 0, UserId = 301009 }, commandType: System.Data.CommandType.StoredProcedure);
        }

        [Benchmark]
        public async Task ProcedureShrinked()
        {
            await _dapperConnection.QueryAsync<AvailableOrganizationStruct>("GetOrganizationsTemp", new { Count }, commandType: System.Data.CommandType.StoredProcedure);
        }

        [Benchmark]
        public async Task RawSql()
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

            await _dapperConnection.QueryAsync<OrganizationModel>(sql, new { Count });
        }

        [Benchmark]
        public async Task EfCore()
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

            await _dbContext.Organizations
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
        public string IdentificationCode { get; set; }
        public string ManagerDisplayName { get; set; }
        public string OrganizationName { get; set; }
        public string NegotiationStatus { get; set; }
        public string Product { get; set; }
        public string ProductType { get; set; }
        public string? Comment { get; set; }
        public DateTime ContactedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }

}
