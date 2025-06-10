using Core.Rabbit.Abstract;

namespace Core.Rabbit;

public class RabbitBasicDirectConsumer : RabbitRootConsumer
{
    public RabbitBasicDirectConsumer(
        string name,
        string queue,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(name, queue, host, username, password, port) { }
}
