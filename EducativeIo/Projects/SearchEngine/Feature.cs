using Core.Guards;

namespace EducativeIo.Projects.SE
{
    public class SearchEngine
    {
        private readonly WordDictionary _wd;
        private AutoCompleteSystem? _acs;
        public SearchEngine()
        {
            _wd = new WordDictionary();
        }
        public void Insert(string word) => _wd.Insert(word);
        public bool ContainsWord(string word) => _wd.ContainsWord(word);
        public bool StartsWith(string word) => _wd.StartsWith(word);
        public List<string> BreakQuery(string query, string[] sentences) => AutoCompleteSystem.BreakQuery(query, sentences);

        public SearchEngine StartAutoComplete(string[] sentences, int[] times)
        {
            _acs = new AutoCompleteSystem(sentences, times);
            return this;
        }
        public SearchEngine AddSentence(string sentence, int t)
        {
            Guard.AgainstNull(_acs);
            _acs.Add(sentence, t);
            return this;
        }
        public List<string> Complete(char c)
        {
            Guard.AgainstNull(_acs);
            return _acs.AutoComplete(c);
        }
        public void ResetSearch()
        {
            Guard.AgainstNull(_acs);
            _acs.ResetSearch();
        }
        public bool BrokenQuery(string query, string[] words)
        {
            HashSet<string> wordsSet = [.. words];
            int length = query.Length;
            bool[] dp = new bool[length + 1];
            dp[0] = true;
            for (int i = 1; i <= length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    string sub = query[j..i];
                    if (dp[j] && wordsSet.Contains(sub))
                    {
                        dp[i] = true;
                    }
                }
            }

            return dp[length];
        }
        private class WordDictionary
        {
            private readonly Node _root;
            public WordDictionary()
            {
                _root = new Node();
            }

            public void Insert(string word)
            {
                Node node = _root;
                foreach (char c in word.ToCharArray())
                {
                    if (!node.Children.TryGetValue(c, out Node? value))
                    {
                        value = new Node();
                        node.Children[c] = value;
                    }

                    node = value;
                }
                node.MarkEndWord();
            }

            public bool ContainsWord(string word)
            {
                Node node = _root;
                foreach (char c in word.ToCharArray())
                {
                    if (!node.Children.TryGetValue(c, out Node? value))
                    {
                        return false;
                    }

                    node = value;
                }
                return node.IsEndWord;
            }

            public bool StartsWith(string word)
            {
                Node node = _root;
                foreach (char c in word.ToCharArray())
                {
                    if (!node.Children.TryGetValue(c, out Node? value))
                    {
                        return false;
                    }

                    node = value;
                }

                return true;
            }
        }
        private class AutoCompleteSystem
        {
            private Node _root;
            private Node _current;
            private string _keyWord;
            private readonly char _delimeter;
            public AutoCompleteSystem(string[] sentences, int[] times)
            {
                if (sentences.Length != times.Length)
                {
                    throw new InvalidOperationException();
                }

                _root = new Node();
                _current = _root;
                _keyWord = string.Empty;
                _delimeter = '#';

                for (int i = 0; i < sentences.Length; i++)
                {
                    Add(sentences[i], times[i]);
                }
            }

            public void Add(string sentence, int t)
            {
                Node node = _root;
                List<Node> visited = [];
                foreach (char c in sentence.ToCharArray())
                {
                    if (!node.Children.TryGetValue(c, out Node? value))
                    {
                        node.Children[c] = new Node();
                    }
                    node = node.Children[c];
                    visited.Add(node);
                }

                node.MarkEndWord();
                node.Data = sentence;
                node.Rank += t;

                foreach (Node n in visited)
                {
                    n.Update(node);
                }
            }
            public void ResetSearch()
            {
                _current = _root;
                _keyWord = string.Empty;
            }
            public List<string> AutoComplete(char c)
            {
                List<string> res = [];
                if (c == _delimeter)
                {
                    Add(_keyWord, 1);
                    _keyWord = string.Empty;
                    _current = _root;
                    return res;
                }

                _keyWord += c;
                if (_current is not null)
                {
                    if (!_current.Children.TryGetValue(c, out Node? value))
                    {
                        return res;
                    }
                    else
                    {
                        _current = value;
                    }
                }
                else
                {
                    return res;
                }

                foreach (Node node in _current.Hot)
                {
                    res.Add(node.Data);
                }

                return res;
            }

            public static List<string> BreakQuery(string query, string[] sentences)
            {
                HashSet<string> hashSet = [.. sentences];
                return BreakQueryHelper(query, hashSet, []);
            }
            private static List<string> BreakQueryHelper(string query, HashSet<string> sentences, Dictionary<string, List<string>> map)
            {
                if (map.TryGetValue(query, out List<string>? value))
                {
                    return value;
                }

                List<string> res = [];

                if (query.Length == 0)
                {
                    res.Add("");
                    return res;
                }

                foreach (string word in sentences)
                {
                    if (!query.StartsWith(word))
                    {
                        continue;
                    }

                    List<string> sublist = BreakQueryHelper(query[word.Length..], sentences, map);
                    foreach (string sub in sublist)
                    {
                        res.Add($"{word}{(string.IsNullOrEmpty(sub) ? "" : " ")}{sub}");
                    }
                }
                map[query] = res;
                return res;
            }
        }
        private class Node : IComparable<Node>
        {
            public Dictionary<char, Node> Children
            {
                get;
            }
            public bool IsEndWord
            {
                get; set;
            }
            public int Rank
            {
                get; set;
            }
            public string Data
            {
                get; set;
            }
            public List<Node> Hot
            {
                get; set;
            }
            private const int HotCount = 2;
            public Node()
            {
                Children = [];
                IsEndWord = false;
                Rank = -1;
                Data = string.Empty;
                Hot = [];
            }
            public bool MarkEndWord() => IsEndWord = true;

            public int CompareTo(Node? other)
            {
                if (other is null)
                {
                    return 1;
                }

                if (Rank == other.Rank)
                {
                    return Data.CompareTo(other.Data);
                }

                return other.Rank - Rank;
            }
            public void Update(Node node)
            {
                if (!Hot.Contains(node))
                {
                    Hot.Add(node);
                }

                Hot.Sort();
                if (Hot.Count > HotCount)
                {
                    Hot.Remove(Hot[^1]);
                }
            }
        }
    }
}