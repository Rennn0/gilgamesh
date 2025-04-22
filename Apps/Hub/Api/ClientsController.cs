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
        private readonly ApplicationContext m_db;
        private readonly ILogger<ClientsController> m_logger;

        public ClientsController(ApplicationContext dbContext, ILogger<ClientsController> logger)
        {
            m_db = dbContext;
            m_logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Client client)
        {
            m_db.Clients.Add(client);
            await m_db.SaveChangesAsync();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Client? msg = await m_db.Clients.FindAsync(id);
            if (msg == null)
                return NotFound();
            m_db.Clients.Remove(msg);
            await m_db.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await m_db.Clients.ToListAsync());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Excel()
        {
            var tempFilePath = Path.Combine(Path.GetTempPath(), $"clients_{Guid.NewGuid()}.xlsx");
            m_logger.LogInformation($"Temp file path: {tempFilePath}");

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
        public async Task<IActionResult> DownloadLargeFile()
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
