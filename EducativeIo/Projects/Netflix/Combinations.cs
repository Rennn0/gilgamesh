using System.Globalization;

namespace EducativeIo.Projects.Netflix
{
    public static class StackExtensions
    {
        public static bool IsEmpty<T>(this Stack<T> stack) => stack.Count == 0;
    }
    public partial class Netflix
    {
        public class ContinueWatching
        {
            private int _maxFrequency;
            private readonly Dictionary<string, int> _titleFrequencyMap;
            private readonly Dictionary<int, Stack<string>> _frequencyTitlesMap;
            public ContinueWatching()
            {
                _maxFrequency = 0;
                _titleFrequencyMap = new Dictionary<string, int>();
                _frequencyTitlesMap = new Dictionary<int, Stack<string>>();
            }

            public void Add(string title)
            {
                if (_titleFrequencyMap.TryGetValue(title, out int value))
                {
                    _titleFrequencyMap[title] = ++value;
                }
                else
                {
                    _titleFrequencyMap.Add(title, 1);
                }

                if (_titleFrequencyMap[title] > _maxFrequency)
                {
                    _maxFrequency = _titleFrequencyMap[title];
                }

                if (_frequencyTitlesMap.TryGetValue(_titleFrequencyMap[title], out Stack<string>? titles))
                {
                    titles.Push(title);
                }
                else
                {
                    _frequencyTitlesMap.Add(_titleFrequencyMap[title], new Stack<string>([title]));
                }
            }

            public string GetMovie()
            {
                if (!_frequencyTitlesMap.TryGetValue(_maxFrequency, out Stack<string>? titles))
                    return string.Empty;

                string title = titles.Pop();

                _titleFrequencyMap[title]--;
                if (_frequencyTitlesMap[_maxFrequency].IsEmpty())
                {
                    _maxFrequency--;
                }

                return title;
            }
        }
        public class MovieCombinations
        {
            private readonly Dictionary<string, string[]> _movies;
            private readonly List<string> _combinations;
            public MovieCombinations()
            {
                _combinations = new List<string>();
                _movies = new Dictionary<string, string[]>
                {
                    { "Family", new string[3]{"Frozen","Kung fu Panda", "Ice Age"} },
                    { "Action", new string[3]{"Iron Man","Wonder Woman","Avengers"}},
                    { "Fantasy", new string[3]{"Jumangi", "Lion King", "Tarzan"}},
                    { "Comedy", new string[4]{"Coco", "The Croods", "Vivi","Pets"}},
                    { "Horror", new string[4]{"Oculus", "Sinister","Insidious","Annebelle"}},
                };
            }

            public List<string> Combinations(string[] categories)
            {
                if (categories.Length == 0) return [];
                Combinations(0, new List<string>(), ref categories);
                return _combinations;
            }

            public List<List<string>> Permutations(string[] movies)
            {
                List<List<string>> result = new List<List<string>>();
                // PermutationsBacktrack(movies.Length, 0, movies.ToList(), result);

                Stack<(int start, List<string> current)> stack = new Stack<(int start, List<string> current)>();

                stack.Push((0, movies.ToList()));

                while (!stack.IsEmpty())
                {
                    (int start, List<string> current) = stack.Pop();
                    if (start == current.Count)
                    {
                        result.Add(new List<string>(current));
                        continue;
                    }

                    for (int i = current.Count - 1; i >= start; i--)
                    {
                        List<string> newList = new List<string>(current);
                        (newList[start], newList[i]) = (newList[i], newList[start]);
                        stack.Push((start + 1, newList));
                    }
                }

                return result;
            }

            private void PermutationsBacktrack(int length, int start, List<string> movies, List<List<string>> permutations)
            {
                if (length == start) permutations.Add(new List<string>(movies));

                for (int i = start; i < length; i++)
                {
                    (movies[i], movies[start]) = (movies[start], movies[i]);

                    PermutationsBacktrack(length, start + 1, movies, permutations);

                    (movies[i], movies[start]) = (movies[start], movies[i]);
                }
            }

            private void Combinations(int index, List<string> path, ref string[] categories)
            {
                if (path.Count == categories.Count())
                {
                    _combinations.Add(string.Join("", path));
                    return;
                }

                string[] possibleCombinations = _movies[categories[index]];
                for (int i = 0; i < possibleCombinations.Length; i++)
                {
                    path.Add($"{possibleCombinations[i]};");
                    Combinations(index + 1, path, ref categories);
                    if (path.Count > 0)
                    {
                        path.RemoveAt(path.Count - 1);
                    }
                }
            }
        }
    }
}