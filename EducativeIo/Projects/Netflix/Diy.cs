using System.Diagnostics;
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

            public void InsertNum(int num) => NumHolder.Instance.Nums.Add(num);


            public float FindMedian()
            {
                int count = NumHolder.Instance.Nums.Count;
                List<int> list = NumHolder.Instance.Nums.Order().ToList();
                if (count % 2 == 0)
                {
                    return (list[count / 2] + list[count / 2 - 1]) / 2f;
                }
                else
                {
                    return list[count / 2];
                }
            }

            internal class NumHolder
            {
                internal List<int> Nums = new List<int>();
                private NumHolder() { }
                public static NumHolder Instance = new NumHolder();
            }

            public bool IsMonotonic(int[] arr)
            {
                bool asc = true;
                bool desc = true;

                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (arr[i] < arr[i + 1])
                    {
                        desc = false;
                    }

                    if (arr[i] > arr[i + 1])
                    {
                        asc = false;
                    }
                }

                return asc || desc;
            }

            public class MinStack
            {
                private int _capacity;
                private readonly Stack<int> _mainStack;
                private readonly Stack<int> _minStack;
                public MinStack(int capacity)
                {
                    _capacity = capacity;
                    _mainStack = new Stack<int>(_capacity);
                    _minStack = new Stack<int>(_capacity);
                }

                public int Pop()
                {
                    _minStack.Pop();
                    return _mainStack.Pop();
                }

                public void Push(int value)
                {
                    if (_mainStack.Count >= _capacity) return;

                    _mainStack.Push(value);
                    int currentMin = _minStack.IsEmpty() ? value : _minStack.Peek();
                    if (value <= currentMin)
                    {
                        _minStack.Push(value);
                    }
                    else
                    {
                        _minStack.Push(currentMin);
                    }
                }

                public int Min()
                {
                    return _minStack.Peek();
                }
            }
        }
    }
}