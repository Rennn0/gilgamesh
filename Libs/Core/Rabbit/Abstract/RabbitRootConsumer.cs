using Core.Guards;
using Core.Rabbit.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.Rabbit.Abstract;

public abstract class RabbitRootConsumer : IDisposable
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
    /// <summary>
    ///     Instantiates single connection to rabbit, creates channel and binds queue to it.
    ///     To attach callbacks on incoming messages use AttachCallback method
    /// </summary>
    /// <param name="prefetchsize"></param>
    /// <param name="prefetchcount"></param>
    /// <param name="global"></param>
    /// <returns></returns>

    [Obsolete("Use ChannelCreationOptions instead")]
    public virtual async Task StartListeningAsync(uint prefetchsize = 0, ushort prefetchcount = 5, bool global = false)
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
        await Channel.BasicQosAsync(prefetchsize, prefetchcount, global);

        Consumer = new AsyncEventingBasicConsumer(Channel);
        await Channel.BasicConsumeAsync(queue: Queue, autoAck: false, consumer: Consumer);

        _listening = true;
    }

    /// <summary>
    ///     Instantiates single connection to rabbit, creates channel and binds queue to it.
    ///     To attach callbacks on incoming messages use AttachCallback method
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public virtual async Task StartListeningAsync(ChannelCreationOptions options = default)
    {
        if (!Root.HasConnection())
            await Root.CreateConnectionAsync();

        if (_listening)
            return;

        Channel = await Root.Connection.CreateChannelAsync();

        await Channel.QueueDeclareAsync(
            queue: Queue,
            durable: options.Durable,
            exclusive: options.Exclusive,
            autoDelete: options.AutoAck
        );
        await Channel.BasicQosAsync(options.Prefetchsize, options.Prefetchcount, options.Global);

        Consumer = new AsyncEventingBasicConsumer(Channel);
        await Channel.BasicConsumeAsync(queue: Queue, autoAck: options.AutoAck, consumer: Consumer);

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

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Consumer = null;
        Channel?.Dispose();
        Channel = null;
    }
}
