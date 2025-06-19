using EducativeIo.Bst;

namespace EducativeIo.Projects.SE
{
    public class Solution
    {
        public int[] FindProduct(int[] arr)
        {
            if (arr.Length == 0)
            {
                return [];
            }

            int[] product = new int[arr.Length];
            product[0] = 1;

            for (int i = 1; i < arr.Length; i++)
            {
                product[i] = arr[i - 1] * product[i - 1];
            }

            int r = 1;

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                product[i] *= r;
                r *= arr[i];
            }

            return product;
        }
        public class PrefixTree<TNode>
        {
            private readonly PrefixTreeNode<TNode> _rootNode;
            private PrefixTreeNode<TNode> _crawlerNode;
            public PrefixTree()
            {
                _rootNode = new PrefixTreeNode<TNode>();
                _crawlerNode = _rootNode;
            }
            private void ResetCrawler() => _crawlerNode = _rootNode;
            public void Insert(string str, TNode treeNode)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return;
                }

                ResetCrawler();
                foreach (char c in str.ToCharArray())
                {
                    if (!_crawlerNode.Children.TryGetValue(c, out PrefixTreeNode<TNode>? node))
                    {
                        node = new PrefixTreeNode<TNode>();
                        _crawlerNode.Children[c] = node;
                    }
                    _crawlerNode = node;
                }
                _crawlerNode.SetResult(treeNode);
            }
            public bool Search(string str, out TNode? result)
            {
                result = default;
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }

                ResetCrawler();
                foreach (char c in str.ToCharArray())
                {
                    if (!_crawlerNode.Children.TryGetValue(c, out PrefixTreeNode<TNode>? node))
                    {
                        return false;
                    }

                    _crawlerNode = node;
                }
                result = _crawlerNode.Data;
                return _crawlerNode.IsEnd;
            }
            public bool SearchPrefix(string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }

                ResetCrawler();
                foreach (char c in str.ToCharArray())
                {
                    if (!_crawlerNode.Children.TryGetValue(c, out PrefixTreeNode<TNode>? node))
                    {
                        return false;
                    }

                    _crawlerNode = node;
                }

                return true;
            }
            private class PrefixTreeNode<T>
            {
                public Dictionary<char, PrefixTreeNode<T>> Children;
                public T? Data
                {
                    get;
                    private set;
                }
                public bool IsEnd
                {
                    get;
                    private set;
                }
                public PrefixTreeNode()
                {
                    Children = [];
                    Data = default;
                }
                public void MarkEnd() => IsEnd = true;
                public void SetResult(T node, bool mark = true)
                {
                    Data = node;
                    if (mark)
                    {
                        MarkEnd();
                    }
                }
            }
        }
    }
}