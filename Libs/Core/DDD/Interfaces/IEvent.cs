
namespace Core.DDD.Interfaces
{
    public interface IEvent
    {
        DateTimeOffset Ts { get; }
    }
}