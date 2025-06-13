using System.Collections.Concurrent;
using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Api
{
    [Route("api/sse")]
    [ApiController]
    public class ServerEventsController : ControllerBase
    {
        const int CMaxClients = 100;
        private static readonly ConcurrentDictionary<Guid, HttpResponse> SClients = new();
        private readonly ILogger<ServerEventsController> _mLogger;

        private static readonly ConcurrentDictionary<string, Channel<string>> SChannels = new();
        private static readonly object SLock = new();

        public ServerEventsController(ILogger<ServerEventsController> logger)
        {
            _mLogger = logger;
        }

        [HttpGet("subscribe/{clientId}")]
        public async Task SubscribeAsync(string clientId)
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
                        _mLogger.LogInformation($"Client {clientId} heartbeat");
                        await Response.WriteAsync(
                            $"event:heartbeat\ndata:{DateTime.Now.ToLocalTime()}\n\n",
                            HttpContext.RequestAborted
                        );
                        await Response.Body.FlushAsync(HttpContext.RequestAborted);
                    }
                    catch (TaskCanceledException)
                    {
                        _mLogger.LogInformation($"Client {clientId} disconnected");
                    }
                    catch (Exception e)
                    {
                        _mLogger.LogInformation($"Error {e.Message}");
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
                    _mLogger.LogInformation($"Client {clientId} received message: {message}");
                    await Response.WriteAsync($"data:{message}\n\n", HttpContext.RequestAborted);
                    await Response.Body.FlushAsync(HttpContext.RequestAborted);
                }
            }
            catch (Exception ex)
            {
                _mLogger.LogInformation($"Client {clientId} disconnected");
                _mLogger.LogInformation(ex, "Error in SSE connection");
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
            lock (SLock)
            {
                if (!SChannels.TryGetValue(clientId, out Channel<string>? channel))
                    return NotFound();

                channel.Writer.TryWrite(message);
                return Ok();
            }

            return NotFound();
        }

        [HttpGet]
        public async Task GetAsync()
        {
            if (SClients.Count >= CMaxClients)
            {
                Response.StatusCode = 509;
                return;
            }

            Response.Headers.Append("Content-Type", "text/event-stream");
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");
            await Response.Body.FlushAsync();

            Guid guid = Guid.NewGuid();

            SClients.TryAdd(guid, Response);
            _mLogger.LogInformation($"Client connected: {guid}");
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
                _mLogger.LogInformation($"Client disconnected: {guid}");
            }
            catch (System.Exception ex)
            {
                _mLogger.LogError(ex, "Error in SSE connection");
            }
            finally
            {
                SClients.TryRemove(guid, out _);
                await Response.CompleteAsync();
            }
        }

        public static async Task PublishAsync(string msg)
        {
            if (SClients.IsEmpty)
                return;

            Stack<Guid> toRemove = new Stack<Guid>();
            IEnumerable<Task> publishTasks = SClients.Select(async kvp =>
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
                SClients.TryRemove(guid, out _);
            }
        }

        private static Channel<string> GetOrCreateChannel(string client)
        {
            lock (SLock)
            {
                if (SChannels.TryGetValue(client, out Channel<string>? channel))
                    return channel;
                channel = Channel.CreateUnbounded<string>();
                SChannels[client] = channel;

                return channel;
            }
        }

        private void RemoveChannel(string client)
        {
            lock (SLock)
            {
                if (!SChannels.TryGetValue(client, out Channel<string>? channel))
                    return;
                channel.Writer.TryComplete();
                SChannels.TryRemove(client, out _);
            }
        }
    }
}
