namespace EducativeIo.Projects.StockScrapper
{
    public class Feature
    {
        public class  TreeNode
        {
            public string Data;
            public int Value;
            public TreeNode? Next;
            public List<TreeNode> Children;
            public TreeNode(string data)
            {
                Data = data;
                Children = new List<TreeNode>();
            }
            public TreeNode(int val)
            {
                Value = val;
                Data = string.Empty;
                Children = new List<TreeNode>();
            }
            public static implicit operator TreeNode(int val) => new TreeNode(val);
        }

        public List<List<string>> Traverse(TreeNode root)
        {
            List<List<string>> result = new List<List<string>>();
            Queue<TreeNode> q = new Queue<TreeNode>();
            q.Enqueue(root);

            while (q.Count > 0)
            {
                int lvlSize = q.Count;
                List<string> thisLvl = new List<string>(lvlSize);
                for (int i = 0; i < lvlSize; i++)
                {
                    TreeNode node = q.Dequeue();
                    thisLvl.Add(node.Data);
                    node.Children.ForEach(n => q.Enqueue(n));
                }
                result.Add(thisLvl);
            }

            return result;
        }

        public int LowestCommonAncestor(TreeNode root, TreeNode? a, TreeNode? b)
        {
            Dictionary<TreeNode, TreeNode?> parents = new Dictionary<TreeNode, TreeNode?>();
            Stack<TreeNode> s = new Stack<TreeNode>();
            s.Push(root);
            parents[root] = null;

            while (!parents.ContainsKey(a) || !parents.ContainsKey(b))
            {
                TreeNode node = s.Pop();
                foreach (TreeNode child in node.Children)
                {
                    parents[child] = node;
                    s.Push(child);
                }
            }

            HashSet<TreeNode> ancestors = new HashSet<TreeNode>();
            while (a != null)
            {
                ancestors.Add(a);
                a = parents[a];
            }

            while (!ancestors.Contains(b))
            {
                b = parents[b];
            }

            return b.Value;
        }

        public TreeNode? TraverseDom(TreeNode? root)
        {
            ArgumentNullException.ThrowIfNull(root);

            TreeNode? current = root;
            TreeNode? leftMost = root;
            TreeNode? previous = null;

            while (leftMost is not null)
            {
                current = leftMost;
                leftMost = null;
                previous = null;

                while (current is not null)
                {
                    Console.WriteLine($"Node {current.Value}");
                    // [ ] do somethin with it

                    foreach (TreeNode child in current.Children)
                    {
                        if (previous is not null)
                        {
                            previous.Next = child;
                        }
                        else
                        {
                            leftMost = child;
                        }

                        previous = child;
                    }

                    current = current.Next;
                }
            }

            return root;
        }
    }
}