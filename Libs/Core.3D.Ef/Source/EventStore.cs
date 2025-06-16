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
        public async Task<IList<(int seqNum,IEvent e)>> LoadEventsAync(Guid correlation)
        {
            List<EventModel> eventRecords = await _context.Events.Where(e => e.Correlation == correlation)
                .OrderBy(e => e.SequenceNumber).ToListAsync();

            List<(int seqNum,IEvent e)> events = [];
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
                events.Add((data.SequenceNumber,@event));
            }

            return events;
        }


        public async Task SaveChangesAsync(Guid correlation, IList<(int seqNum, IEvent e)> events)
        {
            if (events.Count == 0) return;

            foreach ((int seqNum, IEvent @event) in events)
            {
                EventModel eventData = new EventModel
                {
                    Correlation = correlation,
                    EventType = @event.GetType().AssemblyQualifiedName ?? "",
                    EventDataJson = JsonConvert.SerializeObject(@event, Formatting.Indented),
                    SequenceNumber = seqNum,
                    Timestamp = @event.Ts.ToUnixTimeMilliseconds()
                };
                _context.Events.Add(eventData);
            }
            await _context.SaveChangesAsync();
        }
    }
}