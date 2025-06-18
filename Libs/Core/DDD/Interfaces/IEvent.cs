
namespace Core.DDD.Interfaces
{
    public interface IEvent
    {
        public DateTimeOffset Ts
        {
            get;
        }
    }
}