namespace EducativeIo.Bst;

public partial class Tree
{
    public void Insert(int value)
    {
        if (m_root == null)
        {
            m_root = new Node(value);
        }
        else
        {
            Insert(m_root, value);
        }
    }

    private Node Insert(Node? node, int value)
    {
        if (node == null)
        {
            return new Node(value);
        }

        if (node.Value < value)
        {
            node.Right = Insert(node.Right, value);
        }
        else
        {
            node.Left = Insert(node.Left, value);
        }

        return node;
    }

    public void _Insert(int value)
    {
        if (m_root == null)
        {
            m_root = new Node(value);
            return;
        }

        Node? current = m_root;
        Node parent = new Node(-1);
        while (current != null)
        {
            parent = current;
            current = current.Value > value ? current.Left : current.Right;
        }

        if (parent.Value > value)
        {
            parent.Left = new Node(value);
        }
        else
        {
            parent.Right = new Node(value);
        }
    }
}
