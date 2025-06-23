using System.Collections.ObjectModel;
using Core.DDD.Interfaces;
using Newtonsoft.Json;

namespace Core.DDD.Abstract
{
    public abstract class RootDomain
    {
        public Guid Correlation
        {
            get; protected set;
        }
        public int SequenceNumber { get; private set; } = -1;

        [JsonIgnore]
        private readonly List<(int seqNum, IEvent e)> _uncommitedEvents = [];

        protected RootDomain()
        {
            Correlation = Guid.NewGuid();
        }
        protected RootDomain(IEnumerable<IEvent> events)
        {
            if (!events.Any())
            {
                throw new InvalidOperationException();
            }

            foreach (IEvent @event in events)
            {
                ApplyEvent(@event);
            }
        }
        public ReadOnlyCollection<IEvent> GetUncommitedEvents() => new ReadOnlyCollection<IEvent>((IList<IEvent>) _uncommitedEvents.Select(e => e.e));

        public async Task CommitEventsAsync(IEventStore eventStore)
        {
            await eventStore.SaveChangesAsync(Correlation, _uncommitedEvents);
            _uncommitedEvents.Clear();
        }

        public static Task<T> ReflectAsync<T>(IEventStore eventStore, string correlation) where T : RootDomain
        => ReflectAsync<T>(eventStore, Guid.Parse(correlation));

        public static async Task<T> ReflectAsync<T>(IEventStore eventStore, Guid correlation) where T : RootDomain
        {
            IList<(int seqNum, IEvent e)> events = await eventStore.LoadEventsAync(correlation);
            if (events.Count == 0)
            {
                throw new InvalidOperationException();
            }

            System.Reflection.ConstructorInfo constructor = typeof(T).GetConstructor([typeof(IEnumerable<IEvent>)]) ?? throw new InvalidOperationException();

            T domain = (T) constructor.Invoke([events.Select(evs => evs.e)]);
            domain.SequenceNumber = events.Max(x => x.seqNum);
            return domain;
        }

        private void ApplyEvent(IEvent @event) => ((dynamic) this).On((dynamic) @event);

        protected void ApplyNewEvent(IEvent @event)
        {
            SequenceNumber++;

            ApplyEvent(@event);
            _uncommitedEvents.Add((SequenceNumber, @event));
        }
    }
}