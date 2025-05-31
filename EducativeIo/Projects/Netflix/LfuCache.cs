using System.Security.Cryptography.X509Certificates;

namespace EducativeIo.Projects.Netflix
{
    public partial class Netflix
    {
        public class LfuCache
        {
            private readonly int _capacity;
            private readonly LinkedList<int> _cache;
            private readonly Dictionary<int, LinkedListNode<int>> _nodeMap;
            private readonly Dictionary<int, int> _freqMap;
            public LfuCache(int capacity)
            {
                _capacity = capacity;
                _cache = new LinkedList<int>();
                _nodeMap = new Dictionary<int, LinkedListNode<int>>(capacity);
                _freqMap = new Dictionary<int, int>(capacity);
            }

            public void Add(int cacheKey)
            {
                if (_nodeMap.TryGetValue(cacheKey, out LinkedListNode<int>? node))
                {
                    _freqMap[node.Value]++;
                }
                else
                {
                    if (_nodeMap.Count >= _capacity)
                        Eviction();

                    LinkedListNode<int> tempNode = new LinkedListNode<int>(cacheKey);
                    _cache.AddLast(tempNode);
                    _nodeMap[cacheKey] = tempNode;
                    _freqMap[cacheKey] = 1;
                }
            }
            public int[] GetCache() => _cache.ToArray();
            public string[] GetFrequency() => _freqMap.OrderBy(x => x.Key).Select(x => $"{x.Key}:{x.Value}").ToArray();
            private void Eviction()
            {
                int cacheKey = _freqMap.MinBy(x => x.Value).Key;
                LinkedListNode<int> nodeToBeDeleted = _nodeMap[cacheKey];
                _cache.Remove(nodeToBeDeleted);
                _nodeMap.Remove(cacheKey);
                _freqMap.Remove(cacheKey);
            }
        }
    }
}