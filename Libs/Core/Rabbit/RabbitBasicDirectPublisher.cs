using Core.Rabbit.Abstract;

namespace Core.Rabbit;

public class RabbitBasicDirectPublisher : RabbitRootPublisher
{
    public RabbitBasicDirectPublisher(
        string name,
        string routingKey,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(name, exchange: "", routingKey, host, username, password, port) { }
}
