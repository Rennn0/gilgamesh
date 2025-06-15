using Core.DDD.Abstract;
using Core.DDD.Interfaces;

namespace Core.DDD.Generic
{
    public class DomainSession<T> : IAsyncDisposable where T : RootDomain
    {
        private readonly T _domain;
        private readonly IEventStore _store;
        public DomainSession(T domain, IEventStore store)
        {
            _domain = domain;
            _store = store;
        }

        public T Domain => _domain;

        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await _domain.CommitEventsAsync(_store);
        }
    }
}