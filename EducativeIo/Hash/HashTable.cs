using System.Runtime.CompilerServices;
using System.Text;

namespace EducativeIo.Hash;

public class HashTable
{
    private int m_size;
    private int m_slot;
    private const float c_loadFactor = 0.7f;
    private HashEntry?[] m_bucket;

    public HashTable()
    {
        m_size = 0;
        m_slot = 3;
        m_bucket = new HashEntry?[m_slot];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetSize() => m_size;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsEmpty() => m_size == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetIndex(string key)
    {
        int k = key.Aggregate(0, (current, t) => current * 37 + t);

        if (k < 0)
            k *= -1;

        return k % m_slot;
    }

    public void Insert(string key, int value)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        if ((float)m_size / m_slot >= c_loadFactor)
            Resize();

        int hash = GetIndex(key);
        if (m_bucket[hash] is null)
        {
            m_bucket[hash] = new HashEntry(key, value);
        }
        else
        {
            HashEntry? entry = m_bucket[hash];
            while (entry?.Next is not null)
            {
                entry = entry.Next;
            }

            entry!.Next = new HashEntry(key, value);
        }

        m_size++;
    }

    public void Resize()
    {
        m_slot *= 2;
        HashEntry?[] newBucket = new HashEntry?[m_slot];

        for (int i = 0; i < m_slot / 2; i++)
        {
            if (m_bucket[i] is null)
                continue;

            HashEntry? entry = m_bucket[i];
            while (entry is not null)
            {
                int hash = GetIndex(entry.Key);

                HashEntry? newNode = new HashEntry(entry.Key, entry.Value)
                {
                    Next = newBucket[hash],
                };
                newBucket[hash] = newNode;

                entry = entry.Next;
            }
        }

        m_bucket = newBucket;
    }

    public int? Search(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return null;
        int hash = GetIndex(key);
        HashEntry? entry = m_bucket[hash];
        while (entry is not null)
        {
            if (entry.Key == key)
                return entry.Value;
            entry = entry.Next;
        }

        return null;
    }

    public void Delete(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return;

        int hash = GetIndex(key);

        HashEntry? current = m_bucket[hash];
        HashEntry? previous = null;

        while (current is not null)
        {
            if (current.Key == key)
            {
                if (previous is null)
                {
                    m_bucket[hash] = current.Next;
                }
                else
                {
                    previous.Next = current.Next;
                }

                m_size--;
                return;
            }

            previous = current;
            current = current?.Next;
        }
    }

    public string Display()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < m_slot; i++)
        {
            sb.Append($"[{i}] -> ");
            HashEntry? entry = m_bucket[i];
            while (entry is not null)
            {
                sb.Append($"({entry.Key}, {entry.Value}) -> ");
                entry = entry.Next;
            }

            sb.Append("null\n");
        }

        return sb.ToString();
    }

    public string FindSymmetric(int[][] arr, int size)
    {
        StringBuilder sb = new StringBuilder();
        HashSet<(int, int)> set = new HashSet<(int, int)>();
        for (int i = 0; i < size; i++)
        {
            if (set.Contains((arr[i][1], arr[i][0])))
            {
                sb.Append($"({arr[i][1]}, {arr[i][0]})({arr[i][0]}, {arr[i][1]})");
            }
            else
            {
                set.Add((arr[i][0], arr[i][1]));
            }
        }

        sb.Replace('(', '{');
        sb.Replace(')', '}');
        return sb.ToString();
    }

    public string TracePath(Dictionary<string, string> map)
    {
        StringBuilder sb = new StringBuilder();

        string? entry = map.FirstOrDefault(x => !map.ContainsValue(x.Key)).Key;

        if (string.IsNullOrEmpty(entry))
            return sb.ToString();

        while (map.ContainsKey(map[entry]))
        {
            sb.Append($"{entry}->{map[entry]} ");
            entry = map[entry];
        }

        sb.Append($"{entry}->{map[entry]}");

        return sb.ToString();
    }

    public string FindPair(int[] arr)
    {
        //StringBuilder sb = new StringBuilder();

        Dictionary<int, int[]> dic = new Dictionary<int, int[]>();

        for (int i = 0; i < arr.Length - 1; i++)
        {
            for (int j = i + 1; j < arr.Length; j++)
            {
                //dic[$"{{{arr[i]},{arr[j]}}}"] = arr[i] + arr[j];
                int sum = arr[i] + arr[j];
                if (dic.TryGetValue(sum, out int[]? pair))
                {
                    return "{" + $"{pair[0]},{pair[1]}" + "}{" + $"{arr[i]},{arr[j]}" + "}";
                }
                else
                {
                    dic[sum] = [arr[i], arr[j]];
                }
            }
        }

        //List<string> match = (
        //    dic.GroupBy(x => x.Value).FirstOrDefault(x => x.Count() > 1) ?? throw new Exception()
        //)
        //    .Select(x => x.Key)
        //    .ToList();
        //sb.Append('{');
        //sb.Append(string.Join(',', match));
        //sb.Append('}');
        return "";
    }

    public bool FindSubZero(int[] arr)
    {
        // for (int i = 0; i < arr.Length - 1; i++)
        // {
        //     for (int j = i + 1; j < arr.Length; j++)
        //     {
        //         if (SubSum(i, j) == 0)
        //             return true;
        //     }
        // }
        //
        // int SubSum(int l, int r)
        // {
        //     int sum = 0;
        //     for (int k = l; k <= r; k++)
        //     {
        //         sum += arr[k];
        //     }
        //
        //     return sum;
        // }]
        int[] res = new int[2];

        Dictionary<int, int> dic = new Dictionary<int, int>();
        int sum = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            sum += arr[i];
            if (sum == 0 || !dic.TryAdd(sum, i))
                return true;
        }

        return false;
    }

    public int FindFirstUnique(int[] arr)
    {
        Dictionary<int, int> dic = new Dictionary<int, int>();
        for (int i = 0; i < arr.Length; i++)
        {
            if (dic.ContainsKey(arr[i]))
                dic[arr[i]]++;
            else
                dic[arr[i]] = 1;
        }

        for (int i = 0; i < arr.Length; i++)
        {
            if (dic[arr[i]] == 1)
                return arr[i];
        }

        foreach (KeyValuePair<int, int> pair in dic)
        {
            if (pair.Value > 1)
            {
                /*blahblah ezz*/
            }
        }

        return -1;
    }

    public int[] FindSum(int[] arr, int sum)
    {
        int[] res = new int[2];
        HashSet<int> set = new HashSet<int>();

        for (int i = 0; i < arr.Length; i++)
        {
            int diff = sum - arr[i];
            if (set.Contains(diff))
            {
                res[0] = arr[i];
                res[1] = diff;
                return res;
            }

            set.Add(arr[i]);
        }

        EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
        return res;
    }
}