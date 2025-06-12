using System.Text;
using Core.Rabbit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;

namespace Consumer.BackgroundWorkers;

public class CheckinGuardConsumerWorker : BackgroundService
{
    private readonly ILogger<CheckinGuardConsumerWorker> _logger;

    private const int MessageReceivedEid = 2001;
    private const int StartedListeningEid = 1000;

    private readonly RabbitBasicDirectConsumer _consumer;

    public CheckinGuardConsumerWorker(
        IConfiguration configuration,
        ILogger<CheckinGuardConsumerWorker> logger
    )
    {
        string queue = configuration["RabbitMQ:Queue"] ?? throw new InvalidOperationException();
        string host = configuration["RabbitMQ:Host"] ?? throw new InvalidOperationException();
        string username =
            configuration["RabbitMQ:Username"] ?? throw new InvalidOperationException();
        string password =
            configuration["RabbitMQ:Password"] ?? throw new InvalidOperationException();
        int port = int.Parse(
            configuration["RabbitMQ:Port"] ?? throw new InvalidOperationException()
        );

        _consumer = new RabbitBasicDirectConsumer(
            nameof(CheckinConsumerWorker),
            queue,
            host,
            username,
            password,
            port
        );

        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.StartListeningAsync();

        _logger.LogInformation(new EventId(StartedListeningEid), "StartedListening");

        _consumer.AttachCallback(CallbackAsync);
    }

    private async Task CallbackAsync(object sender, BasicDeliverEventArgs @event)
    {
        _logger.LogInformation(
            new EventId(MessageReceivedEid),
            $"RECEIVED MESSAGE {Encoding.UTF8.GetString(@event.Body.ToArray())}"
        );

        await _consumer.AcknowledgeAsync(@event.DeliveryTag);
    }
}
