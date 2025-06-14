using System.Text;
using Consumer.Settings;
using Core.Rabbit;
using Core.Rabbit.Options;
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
        private readonly RabbitBasicDirectConsumer _consumer2;
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

            _consumer2 = new RabbitBasicDirectConsumer(
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
            await _consumer.StartListeningAsync(new ChannelCreationOptions(prefetchcount: 0));
            await _consumer2.StartListeningAsync(new ChannelCreationOptions(prefetchcount: 0));

            _logger.LogDebug(new EventId(StartedListeningEid), "StartedListening");

            _consumer.AttachCallback(CallbackAsync);
            _consumer2.AttachCallback(CallbackAsync2);
        }
        private async Task CallbackAsync(object sender, BasicDeliverEventArgs @event)
        {
            _logger.LogDebug(
                new EventId(MessageReceivedEid),
                $"1"
            );
            await _consumer.AcknowledgeAsync(@event.DeliveryTag);
        }

        private async Task CallbackAsync2(object sender, BasicDeliverEventArgs @event)
        {
            _logger.LogDebug(
                new EventId(MessageReceivedEid),
                $"2"
            );
            await _consumer.AcknowledgeAsync(@event.DeliveryTag);
        }
    }
}
