#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace EducativeIo.Projects.StockScrapper
{
    public class Solution
    {
        public class TreeNode
        {
            public int data;
            public TreeNode left;
            public TreeNode right;
            public TreeNode(int d)
            {
                data = d;
                left = null;
                right = null;
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
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
