namespace EducativeIo.Bst;

public partial class Tree
{
    public bool Delete(int value) => Delete(m_root, value);

    private bool Delete(Node? node, int value)
    {
        if (node == null || m_root == null)
            return false;

        Node parent = node;
        while (node != null && node.Value != value)
        {
            parent = node;
            node = node.Value < value ? node.Right : node.Left;
        }

        if (node == null)
            return false;

        if (node.Left == null && node.Right == null)
        {
            return DeleteLeafNode(node, value, parent);
        }

        if (node.Right == null)
        {
            return DeleteWithLeftChild(node, value, parent);
        }

        if (node.Left == null)
        {
            return DeleteWithRightChild(node, value, parent);
        }

        return DeleteWithBothChild(node);
    }

    private bool DeleteWithBothChild(Node node)
    {
        if (node.Right == null)
            return false;
        Node leastNode = FindLeastNode(node.Right);
        int tmp = leastNode.Value;
        Delete(m_root, leastNode.Value);
        node.Value = tmp;

        return true;
    }

    private bool DeleteWithRightChild(Node node, int value, Node parent)
    {
        if (m_root != null && m_root.Value == value)
        {
            m_root.Right = node.Right;
        }
        else if (parent.Value < value)
        {
            parent.Right = node.Right;
        }
        else
        {
            parent.Left = node.Right;
        }

        return true;
    }

    private bool DeleteWithLeftChild(Node node, int value, Node parent)
    {
        if (m_root != null && m_root.Value == value)
        {
            m_root = node.Left;
        }
        else if (parent.Value < value)
        {
            parent.Right = node.Left;
        }
        else
        {
            parent.Left = node.Left;
        }

        return true;
    }

    private bool DeleteLeafNode(Node node, int value, Node parent)
    {
        if (m_root != null && m_root.Value == value)
        {
            m_root = null;
        }
        else if (node.Value < parent.Value)
        {
            parent.Left = null;
        }
        else
        {
            parent.Right = null;
        }

        return true;
    }

    private Node FindLeastNode(Node node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }

        return node;
    }
}
