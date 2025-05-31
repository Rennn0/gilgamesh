namespace EducativeIo.Projects.Netflix
{
    public partial class Netflix
    {

        public class LruCache
        {
            private readonly int _capacity;
            private readonly LinkedList<int> _cache;
            private readonly Dictionary<int, LinkedListNode<int>> _map;
            public LruCache(int capacity)
            {
                _capacity = capacity;
                _cache = new LinkedList<int>();
                _map = new Dictionary<int, LinkedListNode<int>>();
            }

            public void Add(int value)
            {
                if (_map.TryGetValue(value, out LinkedListNode<int>? node))
                {
                    _cache.Remove(node);
                    _cache.AddLast(node);
                }
                else
                {
                    if (_cache.Count >= _capacity)
                    {
                        Eviction();
                    }

                    LinkedListNode<int> temp = new LinkedListNode<int>(value);
                    _cache.AddLast(temp);
                    _map[value] = temp;
                }
            }
            public int[] GetCache() => _cache.ToArray();

            private void Eviction()
            {
                int headVal = _cache.First();
                _map.Remove(headVal);
                _cache.RemoveFirst();
            }
        }
    }
}