using System.Security.Cryptography.X509Certificates;

namespace EducativeIo.Projects.Netflix
{
    public partial class Netflix
    {
        public class LfuCache<T>
        {
            private readonly int _capacity;
            private readonly LinkedList<T> _cache;
            private readonly Dictionary<object, LinkedListNode<T>> _nodeMap;
            private readonly Dictionary<object, int> _freqMap;
            public LfuCache(int capacity)
            {
                _capacity = capacity;
                _cache = new LinkedList<T>();
                _nodeMap = new Dictionary<object, LinkedListNode<T>>(capacity);
                _freqMap = new Dictionary<object, int>(capacity);
            }

            public void Add(object cacheKey, T value)
            {
                if (_nodeMap.TryGetValue(cacheKey, out LinkedListNode<T>? node))
                {
                    _freqMap[cacheKey]++;
                }
                else
                {
                    if (_nodeMap.Count >= _capacity)
                        Eviction();

                    _cache.AddLast(new LinkedListNode<T>(value));
                    _nodeMap[cacheKey] = _cache.Last!;
                    _freqMap[cacheKey] = 1;
                }
            }
            public T[] GetCache() => _cache.ToArray();
            public string[] GetFrequency() => _freqMap.Select(x => $"{x.Key}:{x.Value}").ToArray();
            private void Eviction()
            {
                object cacheKey = _freqMap.MinBy(x => x.Value).Key;
                LinkedListNode<T> nodeToBeDeleted = _nodeMap[cacheKey];
                _cache.Remove(nodeToBeDeleted);
                _nodeMap.Remove(cacheKey);
                _freqMap.Remove(cacheKey);
            }
        }
    }
}