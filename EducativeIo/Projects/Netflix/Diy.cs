using System.Text;

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
        }
    }
}