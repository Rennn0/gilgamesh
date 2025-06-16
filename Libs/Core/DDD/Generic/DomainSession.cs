using Core.DDD.Abstract;
using Core.DDD.Interfaces;

namespace Core.DDD.Generic
{
    public class DomainSession<T> : IAsyncDisposable where T : RootDomain
    {
        private T? _domain;
        private readonly IEventStore _store;
        public DomainSession(IEventStore store)
        {
            _store = store;
        }

        public T Domain => _domain ?? throw new NullReferenceException(nameof(_domain));

        public void AttachDomain(T domain) => _domain = domain;
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            if (_domain is not null)
                await _domain.CommitEventsAsync(_store);
        }
    }
}