using Hub.Database;
using Hub.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Hub.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationContext _mDb;
        private readonly ILogger<ClientsController> _mLogger;

        public ClientsController(ApplicationContext dbContext, ILogger<ClientsController> logger)
        {
            _mDb = dbContext;
            _mLogger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Client client)
        {
            _mDb.Clients.Add(client);
            await _mDb.SaveChangesAsync();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Client? msg = await _mDb.Clients.FindAsync(id);
            if (msg == null)
                return NotFound();
            _mDb.Clients.Remove(msg);
            await _mDb.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _mDb.Clients.ToListAsync());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ExcelAsync()
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), $"clients_{Guid.NewGuid()}.xlsx");
            _mLogger.LogInformation($"Temp file path: {tempFilePath}");

            ExcelPackage.License.SetNonCommercialPersonal("Foo");
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Clients");

                for (int i = 1; i < 500_000; i++)
                {
                    ws.Cells[i, 1].Value = $"Client {i}";
                    ws.Cells[i, 2].Value = $"Client {i} description";
                    ws.Cells[i, 3].Value = $"Client {i} address";
                    ws.Cells[i, 4].Value = $"Client {i} phone";
                }

                await package.SaveAsAsync(new FileInfo(tempFilePath));
            }

            var fs = new FileStream(
                tempFilePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                4096,
                true
            );
            var response = File(
                fs,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "clients.xlsx"
            );
            response.EnableRangeProcessing = true;
            Response.Headers.Append("Accept-Ranges", "bytes");
            Response.Headers.Append("Content-Length", fs.Length.ToString());
            return response;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> DownloadLargeFileAsync()
        {
            var filePath = @"/home/luka/Downloads/clients.xlsx";
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            var response = File(
                fileStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "report.xlsx"
            );
            response.EnableRangeProcessing = true;

            return response;
        }
    }
}
