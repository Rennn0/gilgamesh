using System.Collections.ObjectModel;
using Core.DDD.Interfaces;
using Newtonsoft.Json;

namespace Core.DDD.Abstract
{
    public abstract class RootDomain
    {
        public Guid Correlation { get; protected set; }

        [JsonIgnore]
        private readonly List<IEvent> _uncommitedEvents = [];

        protected RootDomain()
        {
            Correlation = Guid.NewGuid();
        }
        protected RootDomain(IEnumerable<IEvent> events)
        {
            if (!events.Any()) throw new InvalidOperationException();

            foreach (IEvent @event in events.OrderBy(e => e.Timestamp))
            {
                ApplyEvent(@event);
            }
        }
        public ReadOnlyCollection<IEvent> GetUncommitedEvents() => new ReadOnlyCollection<IEvent>(_uncommitedEvents);
        public async Task CommitEventsAsync(IEventStore eventStore)
        {
            ReadOnlyCollection<IEvent> events = GetUncommitedEvents();
            await eventStore.SaveChangesAsync(Correlation, events);
            _uncommitedEvents.Clear();
        }
        public static async Task<T> ReflectAsync<T>(IEventStore eventStore, Guid correlation) where T : RootDomain
        {
            List<IEvent> events = await eventStore.LoadEventsAync(correlation);
            if (events.Count == 0) throw new InvalidOperationException();

            System.Reflection.ConstructorInfo constructor = typeof(T).GetConstructor([typeof(IEnumerable<IEvent>), typeof(Guid)]) ?? throw new InvalidOperationException();
            return (T)constructor.Invoke([events, correlation]);
        }

        private void ApplyEvent(IEvent @event)
        {
            ((dynamic)this).On((dynamic)@event);
        }

        protected void ApplyNewEvent(IEvent @event)
        {
            ApplyEvent(@event);
            _uncommitedEvents.Add(@event);
        }

    }
}