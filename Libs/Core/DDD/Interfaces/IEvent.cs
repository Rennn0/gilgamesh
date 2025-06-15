namespace Core.DDD.Interfaces
{
    public interface IEvent
    {
        DateTimeOffset Timestamp { get; }
    }

}