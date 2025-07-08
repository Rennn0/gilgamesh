using System.Runtime.CompilerServices;
using System.Text;

namespace EducativeIo.Bst;

public partial class Tree
{
    private Node? m_root;

    public Tree()
    {
        m_root = null;
    }

    public Tree(Node node)
    {
        m_root = node;
    }

    public Node? GetRoot()
    {
        return m_root;
    }

    public void InorderPrint(Node? node)
    {
        if (node == null)
            return;
        InorderPrint(node.Left);
        Console.Write($"{node.Value} ");
        InorderPrint(node.Right);
    }

    public int Height() => Height(m_root);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public int Height_()
    {
        if (m_root == null)
            return -1;

        Queue<Node> q = new Queue<Node>();
        int height = 0;
        q.Enqueue(m_root);

        while (q.Count > 0)
        {
            int level = q.Count;
            for (int i = 0; i < level; i++)
            {
                Node current = q.Dequeue();
                if (current.Left != null)
                {
                    q.Enqueue(current.Left);
                }

                if (current.Right != null)
                {
                    q.Enqueue(current.Right);
                }
            }

            height++;
        }

        return --height;
    }

    private int Height(Node? node)
    {
        if (node == null)
            return -1;

        int leftHeight = Height(node.Left);
        int rightHeight = Height(node.Right);
        return Math.Max(leftHeight, rightHeight) + 1;
    }

    public string FindKNodes(int k) => FindKNodes(m_root, k);

    public string FindKNodesRecursive(int k)
    {
        string result = string.Empty;
        FindKNodesRecursive(m_root, k, ref result);
        return result;
    }

    public int Diameter()
    {
        int dia = 0;
        dfs(m_root, ref dia);
        return dia;
        static int dfs(Node? node, ref int d)
        {
            if (node == null)
            {
                return 0;
            }

            int ltree = dfs(node.Left, ref d);
            int rtree = dfs(node.Right, ref d);

            d = Math.Max(d, ltree + rtree);

            return Math.Max(ltree, rtree) + 1;
        }
    }

    private void FindKNodesRecursive(Node? node, int i, ref string result)
    {
        if (node == null)
            return;

        if (i == 0)
        {
            result += $"{node.Value} ";
        }
        else
        {
            FindKNodesRecursive(node.Left, i - 1, ref result);
            FindKNodesRecursive(node.Right, i - 1, ref result);
        }
    }

    private string FindKNodes(Node? node, int k)
    {
        if (node == null)
            return string.Empty;
        Queue<Node> q = new Queue<Node>();
        StringBuilder sb = new StringBuilder();
        int level = 0;

        q.Enqueue(node);
        while (q.Count > 0 && level <= k)
        {
            int size = q.Count;
            for (int i = 0; i < size; i++)
            {
                Node n = q.Dequeue();

                if (level == k)
                {
                    sb.Append($"{n.Value} ");
                }

                if (n.Left != null)
                {
                    q.Enqueue(n.Left);
                }

                if (n.Right != null)
                {
                    q.Enqueue(n.Right);
                }
            }

            level++;
        }

        return sb.ToString();
    }
}
