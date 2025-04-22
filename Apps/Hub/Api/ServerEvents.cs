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
        const int c_maxClients = 100;
        private static readonly ConcurrentDictionary<Guid, HttpResponse> s_clients = new();
        private readonly ILogger<ServerEventsController> m_logger;

        public ServerEventsController(ILogger<ServerEventsController> logger)
        {
            m_logger = logger;
        }

        [HttpGet]
        public async Task Get()
        {
            if (s_clients.Count >= c_maxClients)
            {
                Response.StatusCode = 509;
                return;
            }

            Response.Headers.Append("Content-Type", "text/event-stream");
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");
            await Response.Body.FlushAsync();

            Guid guid = Guid.NewGuid();

            s_clients.TryAdd(guid, Response);
            m_logger.LogInformation($"Client connected: {guid}");
            try
            {
                while (!HttpContext.RequestAborted.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), HttpContext.RequestAborted);
                    await Response.WriteAsync($"event: heartbeat\ndata: {DateTime.UtcNow.ToLocalTime()}\n\n",
                        HttpContext.RequestAborted);
                    await Response.Body.FlushAsync(HttpContext.RequestAborted);
                }
            }
            catch (TaskCanceledException)
            {
                m_logger.LogInformation($"Client disconnected: {guid}");
            }
            catch (System.Exception ex)
            {
                m_logger.LogError(ex, "Error in SSE connection");
            }
            finally
            {
                s_clients.TryRemove(guid, out _);
                await Response.CompleteAsync();
            }
        }

        public static async Task Publish(string msg)
        {
            if (s_clients.IsEmpty)
                return;

            Stack<Guid> toRemove = new Stack<Guid>();
            var publishTasks = s_clients.Select(async kvp =>
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
                s_clients.TryRemove(guid, out _);
            }
        }
    }
}