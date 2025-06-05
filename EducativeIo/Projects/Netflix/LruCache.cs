namespace EducativeIo.Projects.Netflix
{
    public partial class Netflix
    {

        public class LruCache<T> where T : IComparable
        {
            private readonly int _capacity;
            private readonly LinkedList<T> _cache;
            private readonly Dictionary<object, LinkedListNode<T>> _map;
            public LruCache(int capacity)
            {
                _capacity = capacity;
                _cache = new LinkedList<T>();
                _map = new Dictionary<object, LinkedListNode<T>>();
            }

            public void Add(object cacheKey, T value)
            {
                if (_map.TryGetValue(cacheKey, out LinkedListNode<T>? node))
                {
                    _cache.Remove(node);
                    _cache.AddLast(new LinkedListNode<T>(value));
                    _map[cacheKey] = _cache.Last!;
                }
                else
                {
                    if (_cache.Count >= _capacity)
                        Eviction();

                    _cache.AddLast(new LinkedListNode<T>(value));
                    _map[cacheKey] = _cache.Last!;
                }
            }
            public T[] GetCache() => _cache.ToArray();

            private void Eviction()
            {
                LinkedListNode<T>? firstNode = _cache.First;
                if (firstNode is null) return;

                object key = _map.First(x => x.Value.Value.Equals(firstNode.Value)).Key;
                _map.Remove(key);

                _cache.RemoveFirst();
            }
        }
    }
}