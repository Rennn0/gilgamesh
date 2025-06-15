using Core.DDD.Interfaces;

namespace Core.DDD.Abstract
{
    public abstract record Event : IEvent
    {
        public DateTimeOffset Timestamp { get; }
        protected Event()
        {
            Timestamp = DateTimeOffset.UtcNow;
        }
    }

}