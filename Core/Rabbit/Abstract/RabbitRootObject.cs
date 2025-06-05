using RabbitMQ.Client;

namespace Core.Rabbit.Abstract;

public abstract class RabbitRootObject
{
    private static Lazy<IConnection> m_connection;

    protected IChannel? Channel;

    protected RabbitRootObject(string host, string username, string password, int port = 5672)
    {
        ConnectionFactory = new ConnectionFactory()
        {
            UserName = username,
            Password = password,
            HostName = host,
            Port = port,
            AutomaticRecoveryEnabled = true,
        };
    }

    public ConnectionFactory ConnectionFactory { get; }

    public IConnection? Connection => m_connection;
    public virtual bool ChannelIsReady => Channel is not null;

    public virtual async Task InitializeAsync()
    {
        m_connection = await ConnectionFactory.CreateConnectionAsync().ConfigureAwait(false);
    }
}
