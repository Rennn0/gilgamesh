using System.Globalization;
using System.Text;
using EducativeIo.BoundedBuffer;
using EducativeIo.Heap;

namespace EducativeIo.Projects.Netflix
{
    public partial class Netflix
    {
        public class Solution
        {
            public List<List<string>> GroupAnagrams(List<string> args)
            {
                Dictionary<string, List<string>> map = new Dictionary<string, List<string>>();

                args.ForEach(arg =>
                {
                    string hashCode = GetHashCode(arg);
                    if (!map.TryGetValue(hashCode, out List<string>? group))
                    {
                        map.Add(hashCode, new List<string>());
                        map[hashCode].Add(arg);
                    }
                    else
                    {
                        group.Add(arg);
                    }
                });

                return map.Values.ToList();
            }

            private string GetHashCode(string arg)
            {
                const int alphabet = 26;
                int[] buffer = new int[alphabet];

                for (int i = 0; i < arg.Length; i++)
                {
                    buffer[GetIndex(arg[i])]++;
                }

                return HashCodeFromBuffer(buffer);
            }

            private string HashCodeFromBuffer(int[] buffer)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    stringBuilder.Append($"#{buffer[i]}");
                }
                return stringBuilder.ToString();
            }

            private int GetIndex(char c) => c - 'a';

            public LinkedListNode<int>? Merge(List<LinkedListNode<int>> nodes)
            {
                LinkedList<int> linker = new LinkedList<int>();
                MinHeap<int> minHeap = new MinHeap<int>();

                PriorityQueue<int> pq = new PriorityQueue<int>();

                foreach (LinkedListNode<int> node in nodes)
                {
                    LinkedListNode<int>? current = node;
                    while (current != null)
                    {
                        // minHeap.Insert(current.Value);
                        pq.Add(current.Value);
                        current = current.Next;
                    }
                }

                // while (minHeap.Size() != 0)
                while (pq.Size() != 0)
                {
                    // int min = minHeap.GetMin();
                    int min = pq.Poll();
                    linker.AddLast(min);
                    // minHeap.Poll();
                }

                return linker.First;
            }
        }
    }
}