namespace Core.DDD.Interfaces
{
    public interface IEventStore
    {
        Task SaveChangesAsync(Guid correlation, IList<(int seqNum, IEvent e)> events);
        Task<IList<(int seqNum,IEvent e)>> LoadEventsAync(Guid correlation);
    }
}