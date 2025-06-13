using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using Core.Guards;
using Core.Rabbit;
using RabbitMQ.Client.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hub.Backgrounds
{
    public class RabbitQueueWorker : BackgroundService
    {
        private readonly ILogger<RabbitQueueWorker> _logger;
        private readonly IConfiguration _configuration;
        private RabbitBasicDirectConsumer? _consumer;

        public RabbitQueueWorker(ILogger<RabbitQueueWorker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            IsConnected = false;
        }

        public static bool IsConnected { get; private set; }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitializeRabbitMqAsync(stoppingToken);
            Guard.AgainstNull(_consumer);
            if (!IsConnected || !_consumer.IsReady)
            {
                Log(
                    "RabbitMQ channel is not open after initialization attempts. Cannot start consumer."
                );
                return;
            }

            _consumer.AttachCallback(BasicConsumerCallbackAsync);

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            stoppingToken.Register(() => tcs.SetResult(true));
            await tcs.Task;
        }

        private async Task BasicConsumerCallbackAsync(object sender, BasicDeliverEventArgs @event)
        {
            Guard.AgainstNull(_consumer);

            byte[] body = @event.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            Log($"Received message: {message}");

            try
            {
                await _consumer.NegativeAcknowledgeAsync(deliveryTag: @event.DeliveryTag);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Message processing was cancelled due to application shutdown.");
                await _consumer.NegativeAcknowledgeAsync(@event.DeliveryTag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing message: {message}. Nacking message.");

                await _consumer.NegativeAcknowledgeAsync(@event.DeliveryTag, true);
            }
        }

        private async Task InitializeRabbitMqAsync(CancellationToken cancellationToken)
        {
            const int maxRetries = 10;
            int retryCount = 0;
            Random jitter = new Random();
            TimeSpan delay = TimeSpan.FromSeconds(5);

            while (!cancellationToken.IsCancellationRequested && !IsConnected)
            {
                try
                {
                    Log("Rabbit starting...");
                    _consumer = new RabbitBasicDirectConsumer(
                        nameof(RabbitQueueWorker),
                        "Q_1",
                        "localhost",
                        "guest",
                        "guest",
                        5672
                    );

                    await _consumer.StartListeningAsync();
                    Log("Rabbit started successfully");

                    IsConnected = true;
                }
                catch (OperationCanceledException e)
                {
                    IsConnected = false;
                    Log("RabbitMQ initialization cancelled during application shutdown");
                    ExceptionDispatchInfo.Throw(e);
                }
                catch (Exception e)
                {
                    IsConnected = false;
                    Interlocked.Increment(ref retryCount);

                    if (retryCount >= maxRetries)
                    {
                        Log(
                            $"Max RabbitMQ connection retries ({maxRetries}) reached. Consumer will not start."
                        );
                        return;
                    }

                    delay = TimeSpan.FromSeconds(delay.TotalSeconds * 1.3 + jitter.Next(0, 10));
                    Log(
                        $"Failed to initialize RabbitMQ: {e.Message} (attempt {retryCount}/{maxRetries}). Retrying in {delay.TotalSeconds} seconds..."
                    );

                    await Task.Delay(delay, cancellationToken);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Guard.AgainstNull(_consumer);
            await _consumer.DisposeAsync();
            await base.StopAsync(cancellationToken);
        }

        private void Log(
            string message,
            [CallerLineNumber] int callerLine = 0,
            [CallerMemberName] string caller = "",
            [CallerFilePath] string callerFile = ""
        )
        {
            _logger.LogInformation(
                $"{DateTimeOffset.Now:G}_{callerFile}_{caller}_{callerLine}:{Environment.NewLine}{message}"
            );
        }
    }
}
