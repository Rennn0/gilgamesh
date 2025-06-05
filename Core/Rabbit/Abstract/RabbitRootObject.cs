using Core.Guards;
using RabbitMQ.Client;

namespace Core.Rabbit.Abstract;

public class RabbitRootObject
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private readonly object _padLock = new object();
    private static Lazy<RabbitRootObject>? _instance;

    public static RabbitRootObject Instance(Uri uri)
    {
        _instance ??= new Lazy<RabbitRootObject>(() => new RabbitRootObject(uri));
        return _instance.Value;
    }

    public static RabbitRootObject Instance(
        string name,
        string host,
        string username,
        string password,
        int port = 5672
    )
    {
        _instance ??= new Lazy<RabbitRootObject>(
            () => new RabbitRootObject(name, host, username, password, port)
        );
        return _instance.Value;
    }

    static RabbitRootObject() { }

    private RabbitRootObject(Uri uri)
    {
        Monitor.Enter(_padLock);
        _connectionFactory = new ConnectionFactory() { Uri = uri, AutomaticRecoveryEnabled = true };
        Monitor.Exit(_padLock);
    }

    private RabbitRootObject(
        string name,
        string host,
        string username,
        string password,
        int port = 5672
    )
    {
        Monitor.Enter(_padLock);
        _connectionFactory = new ConnectionFactory()
        {
            HostName = host,
            UserName = username,
            Password = password,
            Port = port,
            AutomaticRecoveryEnabled = true,
            ClientProvidedName = name,
        };
        Monitor.Exit(_padLock);
    }

    public void CreateConnection()
    {
        Monitor.Enter(_padLock);

        Guard.AgainstNull(_connectionFactory);
        _connection ??= _connectionFactory.CreateConnectionAsync().Result;

        Monitor.Exit(_padLock);
    }

    public bool HasConnection()
    {
        Monitor.Enter(_padLock);
        try
        {
            return _connection is not null && _connection.IsOpen;
        }
        finally
        {
            Monitor.Exit(_padLock);
        }
    }

    public IConnection Connection =>
        _connection ?? throw new NullReferenceException(nameof(Connection));
}
