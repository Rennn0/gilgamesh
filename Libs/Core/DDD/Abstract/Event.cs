using Core.DDD.Interfaces;

namespace Core.DDD.Abstract
{
    public abstract record Event : IEvent
    {
        public DateTimeOffset Ts { get; }
        protected Event()
        {
            Ts = DateTimeOffset.UtcNow;
        }
    }

}