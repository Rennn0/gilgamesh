namespace Core.DDD.Interfaces
{
    public interface IEventStore
    {
        Task SaveChangesAsync(Guid correlation, IList<(int seqNum, IEvent e)> events);
        Task<IList<IEvent>> LoadEventsAync(Guid correlation);
    }
}