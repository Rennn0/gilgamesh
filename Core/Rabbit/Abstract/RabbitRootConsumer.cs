using Core.Guards;
using RabbitMQ.Client.Events;

namespace Core.Rabbit.Abstract;

public abstract class RabbitRootConsumer : RabbitRootObject
{
    protected readonly string Exchange;
    protected AsyncEventingBasicConsumer? Consumer;

    protected RabbitRootConsumer(
        string exchange,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(host, username, password, port)
    {
        Exchange = exchange;
    }

    public virtual void AttachCallback(AsyncEventHandler<BasicDeliverEventArgs> callback)
    {
        Guard.AgainstNull(Channel);
        Guard.AgainstNull(Consumer);

        Consumer.ReceivedAsync += callback;
    }
}
