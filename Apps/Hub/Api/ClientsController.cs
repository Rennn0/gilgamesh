using System.Text.Json;
using Enyim.Caching.Memcached;
using Hub.Database;
using Hub.Entities;
using Hub.Refit;
using MessagePack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Refit;
using Stripe;
using Stripe.Checkout;

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

        [HttpGet("success/{token:guid}")]
        public IActionResult SuccessfullPayment(Guid token)
        {
            Console.WriteLine($"{token} succed");
            const string domain = "http://172.17.0.1:5500/Apps/Client/success.html";
            Response.Headers.Append("Location", domain);
            return new StatusCodeResult(303);
        }

        [HttpGet("fail/{token:guid}")]
        public IActionResult FailedPayment(Guid token)
        {
            Console.WriteLine($"{token} faile");
            const string domain = "http://172.17.0.1:5500/Apps/Client/cancel.html";
            Response.Headers.Append("Location", domain);
            return new StatusCodeResult(303);
        }

        [HttpPost("checkout")]
        public IActionResult Checkout()
        {
            Guid token = Guid.NewGuid();
            Console.WriteLine($"{token} started");
            SessionCreateOptions options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = "price_1Rbo23HBZMMNglkKvqQS2vV0",
                        Quantity = new Random().Next(30, 50),
                    },
                },
                Mode = "payment",
                SuccessUrl = $"http://localhost:5076/api/clients/success/{token}",
                CancelUrl = $"http://localhost:5076/api/clients/fail/{token}",
            };
            SessionService sessionService = new SessionService();
            Session session = sessionService.Create(options);
            Response.Headers.Append("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpGet("product")]
        public IActionResult Product(string name)
        {
            ProductCreateOptions options = new ProductCreateOptions { Name = name };
            ProductService productService = new ProductService();
            Product product = productService.Create(options);
            return Ok(product);
        }

        [HttpGet("price")]
        public IActionResult Price(ushort amount)
        {
            PriceCreateOptions options = new PriceCreateOptions
            {
                Currency = "gel",
                UnitAmount = amount,
                Product = "prod_SWrgiWMSSCoYJ4",
            };
            PriceService priceService = new PriceService();
            return Ok(priceService.Create(options));
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

        [HttpGet("spawn")]
        public async Task<IActionResult> Spawn(
            [FromServices] IMemcachedClient cacheClient,
            int retry
        )
        {
            // TimerState state = new TimerState
            // {
            //     MaxRetry = retry
            // };
            //
            // Timer timer = new Timer((state) =>
            // {
            //     if (state is not TimerState ts || ts.Timer is null)
            //     {
            //         return;
            //     }
            //     Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}, retry {ts.Counter}, max {ts.MaxRetry}");
            //     if (ts.Increment() >= ts.MaxRetry)
            //     {
            //         ts.Timer.Dispose();
            //     }
            // }, state, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2));
            // state.Timer = timer;
            string? json = await cacheClient.GetAsync<string?>(retry.ToString());

            string[]? val;

            if (string.IsNullOrEmpty(json))
            {
                IRandomStringApi randomStringApi = RestService.For<IRandomStringApi>(
                    new HttpClient() { BaseAddress = new Uri("https://www.randomnumberapi.com/") }
                );
                val = await randomStringApi.GetAsync(50, 90, retry);
                string key = retry.ToString();
                await cacheClient.StoreAsync(
                    StoreMode.Set,
                    key,
                    JsonSerializer.Serialize(val),
                    Expiration.From(TimeSpan.FromMinutes(2))
                );
            }
            else
            {
                val = JsonSerializer.Deserialize<string[]>(json);
            }

            return Ok(val);
        }

        [HttpGet("refit")]
        public async Task<IActionResult> Refit(
            [FromServices] IDocsApi docsApi,
            [FromQuery] string page
        )
        {
            Client obj = new Client { Id = 4, Name = Guid.NewGuid().ToString("d") };
            byte[] clientSerialized = MessagePackSerializer.Serialize(obj);

            Client clientDeserialized = MessagePackSerializer.Deserialize<Client>(clientSerialized);

            string jss = MessagePackSerializer.ToJson(clientSerialized);

            string jsss = MessagePackSerializer.ToJson<Client>(clientDeserialized);

            string jsnat = JsonSerializer.Serialize(obj);

            string jsnatc = Newtonsoft.Json.JsonConvert.SerializeObject(obj);

            // var json = MessagePackSerializer.ConvertToJson(clientDeserialized);

            var doc = await docsApi.GetAsync(page);
            return File(doc, "application/octet-stream");
        }
    }

    public class TimerState
    {
        public int MaxRetry;
        public int Counter = 0;
        public Timer? Timer;

        public int Increment() => Interlocked.Increment(ref Counter);
    }
}


// {
//   "id": "prod_SWrgiWMSSCoYJ4",
//   "object": "product",
//   "active": true,
//   "created": 1750360602,
//   "default_price": null,
//   "description": null,
//   "images": [],
//   "livemode": false,
//   "marketing_features": [],
//   "metadata": {},
//   "name": "vashli",
//   "package_dimensions": null,
//   "shippable": null,
//   "statement_descriptor": null,
//   "tax_code": null,
//   "type": "service",
//   "unit_label": null,
//   "updated": 1750360602,
//   "url": null
// }

// {
//   "id": "price_1Rbo23HBZMMNglkKvqQS2vV0",
//   "object": "price",
//   "active": true,
//   "billing_scheme": "per_unit",
//   "created": 1750360915,
//   "currency": "gel",
//   "currency_options": null,
//   "custom_unit_amount": null,
//   "livemode": false,
//   "lookup_key": null,
//   "metadata": {},
//   "nickname": null,
//   "product": "prod_SWrgiWMSSCoYJ4",
//   "recurring": null,
//   "tax_behavior": "unspecified",
//   "tiers": null,
//   "tiers_mode": null,
//   "transform_quantity": null,
//   "type": "one_time",
//   "unit_amount": 13,
//   "unit_amount_decimal": 13
// }

/*
    https://checkout.stripe.com/c/pay/cs_test_a1SWE0fW7HcxhPEQydJPQzFtVZ6HwoqpEUlws1Wugn39yfPJkP8pWlbhTG#fidkdWxOYHwnPyd1blpxYHZxWjA0VGlnYzNNR19ISEtiaW5OXzJiV102RE5GfzdrNnFRSzBiaDNcaWpuTXxCV21Va1N%2FNERJbndDSlZuQWZMYDFidWhxQklzUWRQfXA1RkgwMWJAfURXZ05uNTUzQ0AwdGg0NScpJ2N3amhWYHdzYHcnP3F3cGApJ2lkfGpwcVF8dWAnPyd2bGtiaWBabHFgaCcpJ2BrZGdpYFVpZGZgbWppYWB3dic%2FcXdwYHgl
*/
