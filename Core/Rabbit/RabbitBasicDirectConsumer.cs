using Core.Guards;
using Core.Rabbit.Abstract;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.Rabbit;

public class RabbitBasicDirectConsumer : RabbitRootConsumer
{
    private string? m_queue;
    private string m_routingKey;

    public RabbitBasicDirectConsumer(
        string routingKey,
        string host,
        string username,
        string password,
        string? queue = null,
        int port = 5672
    )
        : base(exchange: "amq.direct", host, username, password, port)
    {
        m_queue = queue;
        m_routingKey = routingKey;
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        Guard.AgainstNull(Connection);

        Channel = await Connection.CreateChannelAsync().ConfigureAwait(false);
        Consumer = new AsyncEventingBasicConsumer(Channel);

        if (m_queue is null)
        {
            QueueDeclareOk queueDeclareResult = await Channel
                .QueueDeclareAsync(
                    durable: false,
                    exclusive: true,
                    autoDelete: true,
                    arguments: null
                )
                .ConfigureAwait(false);

            m_queue = queueDeclareResult.QueueName;
        }
        else
        {
            await Channel
                .QueueDeclareAsync(
                    queue: m_queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: true
                )
                .ConfigureAwait(false);
        }

        await Channel
            .QueueBindAsync(queue: m_queue, exchange: Exchange, routingKey: m_queue)
            .ConfigureAwait(false);
        await Channel
            .BasicConsumeAsync(queue: m_queue, autoAck: true, consumer: Consumer)
            .ConfigureAwait(false);
    }
}
