using System.Text;
using RabbitMQ.Client;

namespace Core.Rabbit.Abstract;

public abstract class RabbitRootPublisher : IAsyncDisposable
{
    protected readonly string Exchange;
    protected readonly string RoutingKey;
    protected readonly RabbitRootObject Root;
    protected IChannel? Channel;

    protected RabbitRootPublisher(
        string name,
        string exchange,
        string routingKey,
        string host,
        string username,
        string password,
        int port = 5672
    )
    {
        Exchange = exchange;
        RoutingKey = routingKey;

        Root = RabbitRootObject.Instance(name, host, username, password, port);
    }

    public virtual async ValueTask PublishAsync(string message)
    {
        if (!Root.HasConnection())
            await Root.CreateConnectionAsync();
        Channel ??= await Root.Connection.CreateChannelAsync();

        byte[] msgBytes = Encoding.UTF8.GetBytes(message);
        BasicProperties properties = new BasicProperties()
        {
            Persistent = true,
            ContentType = "text/plain",
        };

        await Channel.BasicPublishAsync(
            exchange: Exchange,
            routingKey: RoutingKey,
            basicProperties: properties,
            body: msgBytes,
            mandatory: true
        );
    }

    public virtual async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (Channel is not null)
            await Channel.DisposeAsync();
    }
}
