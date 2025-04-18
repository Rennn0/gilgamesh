using System.Collections.Concurrent;
using Hub.Database;
using Hub.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hub.Api
{
    [Route("api/sse")]
    [ApiController]
    public class ServerEventsController : ControllerBase
    {
        const int MaxClients = 100;
        private static readonly ConcurrentDictionary<Guid, HttpResponse> _clients = new();
        private readonly ILogger<ServerEventsController> _logger;

        public ServerEventsController(ILogger<ServerEventsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task Get()
        {
            if (_clients.Count >= MaxClients)
            {
                Response.StatusCode = 509;
                return;
            }

            Response.Headers.Append("Content-Type", "text/event-stream");
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");
            await Response.Body.FlushAsync();

            Guid guid = Guid.NewGuid();

            _clients.TryAdd(guid, Response);
            _logger.LogInformation($"Client connected: {guid}");
            try
            {
                while (!HttpContext.RequestAborted.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), HttpContext.RequestAborted);
                    await Response.WriteAsync($"event: heartbeat\ndata: {DateTime.UtcNow.ToLocalTime()}\n\n", HttpContext.RequestAborted);
                    await Response.Body.FlushAsync(HttpContext.RequestAborted);
                }
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation($"Client disconnected: {guid}");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error in SSE connection");
            }
            finally
            {
                _clients.TryRemove(guid, out _);
                await Response.CompleteAsync();
            }
        }

        public static async Task Publish(string msg)
        {
            if (_clients.IsEmpty)
                return;

            Stack<Guid> toRemove = new Stack<Guid>();
            var publishTasks = _clients.Select(async kvp =>
            {
                try
                {
                    await kvp.Value.WriteAsync($"data: {msg}\n\n");
                    await kvp.Value.Body.FlushAsync();
                }
                catch (System.Exception)
                {
                    toRemove.Push(kvp.Key);
                }
            });
            await Task.WhenAll(publishTasks);

            foreach (var guid in toRemove)
            {
                _clients.TryRemove(guid, out _);
            }
        }
    }
}
