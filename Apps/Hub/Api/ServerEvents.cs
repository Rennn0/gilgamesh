using System.Collections.Concurrent;
using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api
{
    [Route("api/sse")]
    [ApiController]
    public class ServerEventsController : ControllerBase
    {
        const int c_maxClients = 100;
        private static readonly ConcurrentDictionary<Guid, HttpResponse> s_clients = new();
        private readonly ILogger<ServerEventsController> m_logger;

        private static readonly ConcurrentDictionary<string, Channel<string>> s_channels = new();
        private static readonly object s_lock = new();

        public ServerEventsController(ILogger<ServerEventsController> logger)
        {
            m_logger = logger;
        }

        [HttpGet("subscribe/{clientId}")]
        public async Task Subscribe(string clientId)
        {
            Response.Headers.Append("Content-Type", "text/event-stream");
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");

            Channel<string> channel = GetOrCreateChannel(clientId);

            Timer heartbeat = new Timer(
                async void (_) =>
                {
                    try
                    {
                        m_logger.LogInformation($"Client {clientId} heartbeat");
                        await Response.WriteAsync(
                            $"event:heartbeat\ndata:{DateTime.Now.ToLocalTime()}\n\n",
                            HttpContext.RequestAborted
                        );
                        await Response.Body.FlushAsync(HttpContext.RequestAborted);
                    }
                    catch (TaskCanceledException)
                    {
                        m_logger.LogInformation($"Client {clientId} disconnected");
                    }
                    catch (Exception e)
                    {
                        m_logger.LogInformation($"Error {e.Message}");
                    }
                },
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(10)
            );

            try
            {
                await foreach (
                    string message in channel.Reader.ReadAllAsync(HttpContext.RequestAborted)
                )
                {
                    m_logger.LogInformation($"Client {clientId} received message: {message}");
                    await Response.WriteAsync($"data:{message}\n\n", HttpContext.RequestAborted);
                    await Response.Body.FlushAsync(HttpContext.RequestAborted);
                }
            }
            catch (Exception ex)
            {
                m_logger.LogInformation($"Client {clientId} disconnected");
                m_logger.LogInformation(ex, "Error in SSE connection");
            }
            finally
            {
                RemoveChannel(clientId);
                await Response.CompleteAsync();
                await heartbeat.DisposeAsync();
            }
        }

        [HttpGet("publish/{clientId}")]
        public IActionResult Publish(string clientId, [FromQuery] string message)
        {
            lock (s_lock)
            {
                if (!s_channels.TryGetValue(clientId, out Channel<string>? channel))
                    return NotFound();

                channel.Writer.TryWrite(message);
                return Ok();
            }

            return NotFound();
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
                    await Response.WriteAsync(
                        $"event: heartbeat\ndata: {DateTime.UtcNow.ToLocalTime()}\n\n",
                        HttpContext.RequestAborted
                    );
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
            IEnumerable<Task> publishTasks = s_clients.Select(async kvp =>
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

            foreach (Guid guid in toRemove)
            {
                s_clients.TryRemove(guid, out _);
            }
        }

        private static Channel<string> GetOrCreateChannel(string client)
        {
            lock (s_lock)
            {
                if (s_channels.TryGetValue(client, out Channel<string>? channel))
                    return channel;
                channel = Channel.CreateUnbounded<string>();
                s_channels[client] = channel;

                return channel;
            }
        }

        public void RemoveChannel(string client)
        {
            lock (s_lock)
            {
                if (!s_channels.TryGetValue(client, out Channel<string>? channel))
                    return;
                channel.Writer.TryComplete();
                s_channels.TryRemove(client, out _);
            }
        }
    }
}
