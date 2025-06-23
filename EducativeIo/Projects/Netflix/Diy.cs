using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
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
                private NumHolder()
                {
                }
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

            public class MinMaxStack
            {
                private int _capacity;
                private readonly Stack<int> _mainStack;
                private readonly Stack<int> _minStack;
                private readonly Stack<int> _maxStack;
                public MinMaxStack(int capacity)
                {
                    _capacity = capacity;
                    _mainStack = new Stack<int>(_capacity);
                    _minStack = new Stack<int>(_capacity);
                    _maxStack = new Stack<int>(_capacity);
                }

                public int Pop()
                {
                    _minStack.Pop();
                    _maxStack.Pop();
                    return _mainStack.Pop();
                }

                public void Push(int value)
                {
                    if (_mainStack.Count >= _capacity)
                        return;

                    _mainStack.Push(value);

                    int currentMin = _minStack.IsEmpty() ? value : _minStack.Peek();

                    if (value <= currentMin)
                        _minStack.Push(value);
                    else
                        _minStack.Push(currentMin);

                    int currentMax = _maxStack.IsEmpty() ? value : _maxStack.Peek();
                    if (value >= currentMax)
                        _maxStack.Push(value);
                    else
                        _maxStack.Push(currentMax);
                }

                public int Min() => _minStack.Peek();
                public int Max() => _maxStack.Peek();
            }

            public bool VerifySession(int[] pushed, int[] popped)
            {
                if (pushed.Length != popped.Length)
                    return false;

                int i = 0;
                Stack<int> session = new Stack<int>();
                foreach (int item in pushed)
                {
                    session.Push(item);
                    while (!session.IsEmpty() && popped[i] == session.Peek())
                    {
                        session.Pop();
                        i++;
                    }
                }

                return session.IsEmpty();
            }

            public class Combinations
            {
                private readonly Dictionary<char, string> _map = new()
                    {
                        {'2',"abc"},
                        {'3',"def"},
                        {'4',"ghi"},
                        {'5',"jkl"},
                        {'6',"mno"},
                        {'7',"pqrs"},
                        {'8',"tuv"},
                        {'9',"wxyz"},
                    };
                public string[] LetterCombinatios(string digits)
                {
                    if (string.IsNullOrWhiteSpace(digits))
                    {
                        throw new ArgumentException();
                    }

                    List<string> result = new List<string>();
                    Stack<(int index, string combination)> stack = new Stack<(int index, string combination)>();
                    stack.Push((0, ""));

                    while (!stack.IsEmpty())
                    {
                        (int index, string combination) = stack.Pop();
                        if (index == digits.Length)
                        {
                            result.Add(combination);
                            continue;
                        }

                        string letters = _map[digits[index]];
                        for (int i = letters.Length - 1; i >= 0; i--)
                        {
                            stack.Push((index + 1, $"{combination}{letters[i]}"));
                        }
                    }

                    return result.ToArray();
                }

                public List<List<int>> Permutaions(int[] arr)
                {
                    List<List<int>> result = new List<List<int>>();

                    Stack<(int index, List<int> current)> stack = new Stack<(int index, List<int> current)>();
                    stack.Push((0, arr.ToList()));

                    while (!stack.IsEmpty())
                    {
                        (int index, List<int> current) = stack.Pop();
                        if (index == current.Count)
                        {
                            result.Add(current);
                            continue;
                        }

                        for (int i = current.Count - 1; i >= index; i--)
                        {
                            List<int> temp = new List<int>(current);
                            (temp[i], temp[index]) = (temp[index], temp[i]);
                            stack.Push((index + 1, temp));
                        }
                    }
                    return result;
                }
            }

            public class FreqStack
            {
                private int _maxFreq = 0;
                private Dictionary<int, int> _valFreqMap = new Dictionary<int, int>();
                private Dictionary<int, Stack<int>> _freqValStackMap = new Dictionary<int, Stack<int>>();
                public void Push(int val)
                {
                    if (_valFreqMap.ContainsKey(val))
                        _valFreqMap[val]++;
                    else
                        _valFreqMap[val] = 1;

                    if (_freqValStackMap.ContainsKey(_valFreqMap[val]))
                        _freqValStackMap[_valFreqMap[val]].Push(val);
                    else
                        _freqValStackMap[_valFreqMap[val]] = new Stack<int>([val]);

                    if (_maxFreq < _valFreqMap[val])
                        _maxFreq = _valFreqMap[val];
                }
                public int Pop()
                {

                    int val = _freqValStackMap[_maxFreq].Pop();
                    _valFreqMap[val]--;
                    if (_freqValStackMap[_maxFreq].IsEmpty())
                        _maxFreq--;
                    return val;
                }
            }
        }
    }
}