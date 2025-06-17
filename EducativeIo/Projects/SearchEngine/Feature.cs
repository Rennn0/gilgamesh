namespace EducativeIo.Projects.SE
{
    public class SearchEngine
    {
        private readonly WordDictionary _wd;
        public SearchEngine()
        {
            _wd = new WordDictionary();
        }
        public void Insert(string word) => _wd.Insert(word);
        public bool ContainsWord(string word) => _wd.ContainsWord(word);
        public bool StartsWith(string word) => _wd.StartsWith(word);

        private class WordDictionary
        {
            private Node _root;
            public WordDictionary()
            {
                _root = new Node();
            }

            public void Insert(string word)
            {
                Node node = _root;
                foreach (char c in word.ToCharArray())
                {
                    if (!node.Children.ContainsKey(c))
                        node.Children[c] = new Node();
                    node = node.Children[c];
                }
                node.MarkEndWord();
            }

            public bool ContainsWord(string word)
            {
                Node node = _root;
                foreach (char c in word.ToCharArray())
                {
                    if (!node.Children.TryGetValue(c, out Node? value))
                        return false;
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
                        return false;
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
            public AutoCompleteSystem(string[] sentences,int[] times)
            {

            }
        }
        private class Node : IComparable<Node>
        {
            public Dictionary<char, Node> Children { get; }
            public bool IsEndWord { get; set; }
            public int Rank { get; set; }
            public string Data { get; set; }
            public List<Node> Hot { get; set; }
            private const int HotCount = 3;
            public Node()
            {
                Children = [];
                IsEndWord = false;
                Rank = -1;
                Data = "";
                Hot = [];
            }
            public bool MarkEndWord() => IsEndWord = true;

            public int CompareTo(Node? other)
            {
                if (other is null) return 1;

                if (Rank == other.Rank)
                    return Data.CompareTo(other.Data);

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