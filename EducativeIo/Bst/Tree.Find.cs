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
}
