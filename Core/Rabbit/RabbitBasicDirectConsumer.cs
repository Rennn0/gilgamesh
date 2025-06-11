using Core.Rabbit.Abstract;

namespace Core.Rabbit;

public class RabbitBasicDirectConsumer : RabbitRootConsumer, IAsyncDisposable
{
    public RabbitBasicDirectConsumer(
        string name,
        string queue,
        string host,
        string username,
        string password,
        int port = 5672
    )
        : base(name, queue, host, username, password, port)
    { }
    public bool IsReady => Channel is not null && Channel.IsOpen;
    public async ValueTask DisposeAsync()
    {
        if (Channel is null) return;

        await Channel.DisposeAsync();
        Channel = null;
    }
}
