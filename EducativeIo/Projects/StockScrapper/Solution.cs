#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

using EducativeIo.Bst;

namespace EducativeIo.Projects.StockScrapper
{
    public class Solution
    {
        public class TreeNode
        {
            public int data;
            public TreeNode left;
            public TreeNode right;
            public TreeNode next;

            public TreeNode(int d)
            {
                data = d;
                left = null;
                right = null;
                next = null;
            }
        };

        public static int[][] traverse(TreeNode root)
        {
            List<List<int>> result = new List<List<int>>();
            Queue<TreeNode> nodeQ = new Queue<TreeNode>();
            nodeQ.Enqueue(root);

            while (nodeQ.Count > 0)
            {
                int levelSize = nodeQ.Count;
                List<int> lvl = new List<int>();
                for (int i = 0; i < levelSize; i++)
                {
                    TreeNode n = nodeQ.Dequeue();
                    lvl.Add(n.data);
                    if (n.left != null)
                    {
                        nodeQ.Enqueue(n.left);
                    }

                    if (n.right != null)
                    {
                        nodeQ.Enqueue(n.right);
                    }
                }
                result.Add(lvl);
            }

            return result.Select(r => r.ToArray()).ToArray();
        }

        public static int LCA(TreeNode root, TreeNode node1, TreeNode node2)
        {
            ArgumentNullException.ThrowIfNull(root);
            ArgumentNullException.ThrowIfNull(node1);
            ArgumentNullException.ThrowIfNull(node2);

            TreeNode? current = root;
            TreeNode? leftMost = root;
            TreeNode? previous = null;
            Dictionary<TreeNode, TreeNode> parents = new Dictionary<TreeNode, TreeNode>();
            parents[root] = null;
            while (leftMost != null && (!parents.ContainsKey(node1) || !parents.ContainsKey(node2)))
            {
                current = leftMost;
                leftMost = null;
                previous = null;

                while (current != null)
                {
                    TreeNode[] children = new TreeNode[2] { current.left, current.right };
                    foreach (TreeNode child in children)
                    {
                        if (child != null)
                        {
                            parents[child] = current;
                        }

                        if (previous == null)
                        {
                            leftMost = child;
                        }
                        else
                        {
                            previous.next = child ?? null;
                        }

                        previous = child;
                    }

                    current = current?.next;
                }
            }
            if (!parents.ContainsKey(node1) || !parents.ContainsKey(node2))
            {
                return -1;
            }

            HashSet<TreeNode> ancestors = new HashSet<TreeNode>();
            TreeNode crawler1 = node1;
            while (crawler1 != null)
            {
                ancestors.Add(crawler1);
                crawler1 = parents[crawler1];
            }

            TreeNode crawler2 = node2;
            while (!ancestors.Contains(crawler2))
            {
                crawler2 = parents[crawler2];
            }

            return crawler2.data;
        }

        public static int maxProfit(int[] arr)
        {
            int minPrice = arr[0];
            int maxProfit = 0;
            for (int i = 1; i < arr.Length; i++)
            {
                if (arr[i] < minPrice)
                {
                    minPrice = arr[i];
                }
                else
                {
                    int profit = arr[i] - minPrice;
                    if (profit > maxProfit)
                    {
                        maxProfit = profit;
                    }
                }
            }
            return maxProfit;
        }
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
