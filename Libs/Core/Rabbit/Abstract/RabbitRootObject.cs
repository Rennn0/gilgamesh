using Core.Guards;
using RabbitMQ.Client;

namespace Core.Rabbit.Abstract;

public class RabbitRootObject
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private readonly object _padLock = new object();
    private static readonly object _staticLock = new object();
    private static Lazy<RabbitRootObject>? _instance;
    private readonly Semaphore _sema = new Semaphore(1, 1);

    public static RabbitRootObject Instance(Uri uri)
    {
        try
        {
            Monitor.Enter(_staticLock);
            _instance ??= new Lazy<RabbitRootObject>(() => new RabbitRootObject(uri));
            return _instance.Value;
        }
        finally
        {
            Monitor.Exit(_staticLock);
        }
    }

    public static RabbitRootObject Instance(
        string name,
        string host,
        string username,
        string password,
        int port = 5672
    )
    {
        try
        {
            Monitor.Enter(_staticLock);
            _instance ??= new Lazy<RabbitRootObject>(
                () => new RabbitRootObject(name, host, username, password, port)
            );
            return _instance.Value;
        }
        finally
        {
            Monitor.Exit(_staticLock);
        }
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
            ClientProvidedName = name,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(1)
        };
        Monitor.Exit(_padLock);
    }

    public async Task CreateConnectionAsync()
    {
        Guard.AgainstNull(_connectionFactory);
        try
        {
            _sema.WaitOne();
            _connection = await _connectionFactory.CreateConnectionAsync();
        }
        finally
        {
            _sema.Release();
        }
    }

    public bool HasConnection()
    {
        try
        {
            Monitor.Enter(_padLock);
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
