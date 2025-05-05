using System.Text;

namespace EducativeIo.Bst;

public partial class Tree
{
    public Node? Find(int value) => Find(m_root, value);

    private Node? Find(Node? node, int value)
    {
        if (node == null)
        {
            return null;
        }

        return node.Value == value
            ? node
            : Find(node.Value < value ? node.Right : node.Left, value);
    }

    public Node? _Find(int value)
    {
        if (m_root == null)
        {
            return null;
        }

        Node? current = m_root;
        while (current != null)
        {
            if (current.Value == value)
            {
                return current;
            }

            current = current.Value > value ? current.Left : current.Right;
        }

        return null;
    }

    public int Min() => Min(m_root);

    private int Min(Node? m_root)
    {
        if (m_root == null)
            return -1;

        Node? current = m_root;
        while (current.Left != null)
        {
            current = current.Left;
        }

        return current.Value;
    }

    public int KThLargest(int k)
    {
        if (m_root == null)
            return -1;

        Stack<int> stack = new();
        InOrder(m_root, ref stack);

        if (k > stack.Count)
            return -1;

        int count = 0;
        while (stack.Count > 0)
        {
            int value = stack.Pop();
            count++;
            if (count == k)
                return value;
        }

        return -1;
    }

    public string Ancestors(int k) => Ancestors(m_root, k);

    private string Ancestors(Node? node, int k)
    {
        StringBuilder builder = new StringBuilder();
        if (node == null)
            return builder.ToString();

        while (node != null)
        {
            if (node.Value == k)
            {
                break;
            }

            builder.Insert(0, $"{node.Value.ToString()},");

            node = node.Value < k ? node.Right : node.Left;
        }

        return builder.ToString();
    }
}
