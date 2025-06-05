using System.Text;
using Core.Guards;
using RabbitMQ.Client;

namespace Core.Rabbit.Abstract;

public abstract class RabbitRootPublisher : RabbitRootObject
{
    protected readonly string Exchange;
    protected readonly string RoutingKey;

    protected RabbitRootPublisher(
        string exchange,
        string routingKey,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(host, username, password, port)
    {
        Exchange = exchange;
        RoutingKey = routingKey;
    }

    // TODO basic properties dasamatebelia parametrebshi
    public virtual ValueTask PublishAsync(string message)
    {
        Guard.AgainstNull(Channel);

        byte[] msgBytes = Encoding.UTF8.GetBytes(message);
        BasicProperties properties = new BasicProperties()
        {
            Persistent = true,
            ContentType = "text/plain",
        };
        return Channel.BasicPublishAsync(Exchange, RoutingKey, true, properties, msgBytes);
    }
}
