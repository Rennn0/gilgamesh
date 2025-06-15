using Core.DDD.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Core._3D.Ef
{
    public class EventStore : IEventStore
    {
        private readonly EventStoreContext _context;

        public EventStore(EventStoreContext context)
        {
            _context = context;
        }
        public async Task<List<IEvent>> LoadEventsAync(Guid correlation)
        {
            List<EventModel> eventRecords = await _context.Events.Where(e => e.Correlation == correlation)
                .OrderBy(e => e.Timestamp).ToListAsync();

            List<IEvent> events = [];
            foreach (EventModel data in eventRecords)
            {
                Type? type = Type.GetType(data.EventType);
                if (type is null)
                {
                    Console.WriteLine($"Warning: Could not find event type {data.EventType} for event {data.Id}. Skipping.");
                    continue;
                }

                IEvent? @event = (IEvent?)JsonConvert.DeserializeObject(data.EventDataJson, type);
                if (@event is null) continue;
                events.Add(@event);
            }

            return events;
        }

        public async Task SaveChangesAsync(Guid correlation, IReadOnlyCollection<IEvent> events)
        {
            if (events.Count == 0) return;

            foreach (IEvent @event in events)
            {
                EventModel eventData = new EventModel
                {
                    Correlation = correlation,
                    EventType = @event.GetType().AssemblyQualifiedName ?? "",
                    EventDataJson = JsonConvert.SerializeObject(@event, Formatting.Indented),
                    Timestamp = @event.Timestamp.ToUnixTimeMilliseconds() * 1000 + @event.Timestamp.Ticks % TimeSpan.TicksPerSecond / 10
                };
                _context.Events.Add(eventData);
            }
            await _context.SaveChangesAsync();
        }
    }
}