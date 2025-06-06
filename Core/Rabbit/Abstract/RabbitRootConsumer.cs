using Core.Guards;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.Rabbit.Abstract;

public abstract class RabbitRootConsumer
{
    protected readonly string Queue;
    protected AsyncEventingBasicConsumer? Consumer;
    protected readonly RabbitRootObject Root;
    protected IChannel? Channel;
    private bool _listening;

    protected RabbitRootConsumer(
        string name,
        string queue,
        string host,
        string username,
        string password,
        int port = 5672
    )
    {
        Queue = queue;
        Root = RabbitRootObject.Instance(name, host, username, password, port);

        _listening = false;
    }

    public virtual async Task StartListeningAsync()
    {
        if (!Root.HasConnection())
            await Root.CreateConnectionAsync();

        if (_listening)
            return;

        Channel = await Root.Connection.CreateChannelAsync();
        await Channel.QueueDeclareAsync(
            queue: Queue,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        Consumer = new AsyncEventingBasicConsumer(Channel);
        await Channel.BasicConsumeAsync(queue: Queue, autoAck: false, consumer: Consumer);

        _listening = true;
    }

    public virtual void AttachCallback(AsyncEventHandler<BasicDeliverEventArgs> callback)
    {
        Guard.AgainstNull(Consumer);

        Consumer.ReceivedAsync += callback;
    }

    public virtual async Task AcknowledgeAsync(ulong deliveryTag)
    {
        if (Channel is { IsOpen: true })
        {
            await Channel.BasicAckAsync(deliveryTag, multiple: false);
        }
    }

    public virtual async Task NegativeAcknowledgeAsync(ulong deliveryTag, bool requeue = false)
    {
        if (Channel is { IsOpen: true })
        {
            await Channel.BasicNackAsync(deliveryTag, multiple: false, requeue: requeue);
        }
    }
}
