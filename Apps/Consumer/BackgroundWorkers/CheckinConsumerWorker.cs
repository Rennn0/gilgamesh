using System.Text;
using Consumer.Settings;
using Core.Rabbit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;

namespace Consumer.BackgroundWorkers
{
    public class CheckinConsumerWorker : BackgroundService
    {
        private readonly ILogger<CheckinConsumerWorker> _logger;
        private readonly RabbitBasicDirectConsumer _consumer;
        private const int MessageReceivedEid = 2001;
        private const int StartedListeningEid = 1000;

        public CheckinConsumerWorker(
            ILogger<CheckinConsumerWorker> logger,
            IOptions<RabbitMqSettings> rabbitSettings
        )
        {
            RabbitMqSettings mqSettings = rabbitSettings.Value;

            _consumer = new RabbitBasicDirectConsumer(
                nameof(CheckinConsumerWorker),
                mqSettings.Queue,
                mqSettings.Host,
                mqSettings.Username,
                mqSettings.Password,
                mqSettings.Port
            );

            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.StartListeningAsync();

            _logger.LogDebug(new EventId(StartedListeningEid), "StartedListening");

            _consumer.AttachCallback(CallbackAsync);
        }

        private async Task CallbackAsync(object sender, BasicDeliverEventArgs @event)
        {
            _logger.LogDebug(
                new EventId(MessageReceivedEid),
                $"RECEIVED MESSAGE {Encoding.UTF8.GetString(@event.Body.ToArray())}"
            );
            await _consumer.AcknowledgeAsync(@event.DeliveryTag);
        }
    }
}
