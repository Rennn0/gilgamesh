using System.Reflection.Metadata;
using System.Text;

namespace EducativeIo.Projects.Facebook
{
    public partial class Feature
    {
        public int FriendCicles(bool[][] friends)
        {
            int n = friends.Length;
            if (n == 0) return -1;

            int numCicles = 0;
            bool[] visited = new bool[n];

            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                {
                    visited[i] = true;
                    DFS(friends, visited, i);
                    numCicles++;
                }
            }

            return numCicles;
        }

        public void DFS(bool[][] friends, bool[] visited, int v)
        {
            for (int i = 0; i < friends.Length; i++)
            {
                if (friends[v][i] && !visited[i] && i != v)
                {
                    visited[i] = true;
                    DFS(friends, visited, i);
                }
            }
        }

        public IList<string> WordSubsets(string[] words1, string[] words2)
        {
            // return [.. words1.Where(w1 => words2.All(w2 => IsSubset(w1, w2)))];

            int[] maxFrequency = new int[26];
            foreach (string w2 in words2)
            {
                _ = GetFrequency(w2).Select((f, i) => maxFrequency[i] = Math.Max(maxFrequency[i], f)).ToList();
            }

            List<string> result = new List<string>();

            foreach (string w1 in words1)
            {
                bool isValid = true;

                _ = GetFrequency(w1).Select((f, i) =>
                {
                    if (f < maxFrequency[i])
                    {
                        isValid = false;
                    }
                    return f;
                });

                if (isValid)
                    result.Add(w1);
            }

            return result;
        }
        public int[] GetFrequency(string s)
        {
            int[] freq = new int[26];
            _ = s.Select((c, i) => freq[i] = c - 'a');
            return freq;
        }
        public bool IsSubset(string a, string b)
        {
            Dictionary<char, int> aMap = new Dictionary<char, int>();
            Dictionary<char, int> bMap = new Dictionary<char, int>();


            for (int i = 0; i < a.Length; i++)
            {
                if (aMap.TryGetValue(a[i], out _))
                    aMap[a[i]]++;
                else
                    aMap[a[i]] = 1;
            }

            for (int i = 0; i < b.Length; i++)
            {
                if (bMap.TryGetValue(b[i], out _))
                    bMap[b[i]]++;
                else
                    bMap[b[i]] = 1;
            }

            for (int i = 0; i < b.Length; i++)
            {
                if (!aMap.TryGetValue(b[i], out int value) || value < bMap[b[i]])
                    return false;
            }

            return true;
        }

        public int SearchInCircularList(int[] arr, int key) => SearchInCircularList(arr, key, 0, arr.Length - 1);
        // [6, 7, 1, 2, 3, 4, 5]
        // [4, 5, 6, 7, 1, 2, 3]
        private int SearchInCircularList(int[] arr, int key, int start, int end)
        {
            if (start > end) return -1;

            int mid = start + (end - start) / 2; // 4 6 => 5
            if (arr[mid].Equals(key)) return mid;

            if (arr[start] <= arr[mid] && key >= arr[start] && key <= arr[mid])
                return SearchInCircularList(arr, key, start, mid - 1);
            else if (arr[end] >= arr[mid] && key >= arr[mid] && key <= arr[end])
                return SearchInCircularList(arr, key, mid + 1, end);
            else if (arr[start] >= arr[mid])
                return SearchInCircularList(arr, key, start, mid - 1);
            else if (arr[end] <= arr[mid])
                return SearchInCircularList(arr, key, mid + 1, end);

            return -1;
        }
        // [1,2,3,4,5] → [3,4,5,1,2]
        public void RotateLeft(int[] arr, int n)
        {
            n %= arr.Length;
            arr.Reverse(0, n - 1); // [2,1,3,4,5]
            arr.Reverse(n, arr.Length - 1); // [2,1,5,4,3]
            arr.Reverse(0, arr.Length - 1); // [3,4,5,1,2]
        }
        // [1,2,3,4,5] → [4,5,1,2,3]
        public void RotateRight(int[] arr, int n)
        {
            n %= arr.Length;
            arr.Reverse(0, arr.Length - 1); // [5,4,3,2,1]
            arr.Reverse(0, n - 1); // [4,5,3,2,1]
            arr.Reverse(n, arr.Length - 1); // [4,5,1,2,3]
        }
    }

    public static class Extensions
    {
        public static void Reverse(this int[] arr, int start, int end)
        {
            while (start < end)
            {
                (arr[start], arr[end]) = (arr[end], arr[start]);
                start++;
                end--;
            }
        }
    }
}