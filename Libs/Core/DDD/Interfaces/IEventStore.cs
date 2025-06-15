namespace Core.DDD.Interfaces
{
    public interface IEventStore
    {
        Task SaveChangesAsync(Guid correlation, IReadOnlyCollection<IEvent> events);
        Task<List<IEvent>> LoadEventsAync(Guid correlation);
    }
}