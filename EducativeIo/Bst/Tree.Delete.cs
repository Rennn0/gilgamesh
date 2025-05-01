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
            return DeleteWithoutChildNodes(parent, node);

        if (node.Left == null)
            return DeleteWithRightChildNode(parent, node);

        if (node.Right == null)
            return DeleteWithLeftChildNode(parent, node);

        return DeleteWithBothChildNode(parent, node);
    }

    private bool DeleteWithBothChildNode(Node parent, Node node)
    {
        if (node.Right == null)
            return false;

        Node leastNode = FindLeastNode(node.Right);
        int leastValue = leastNode.Value;
        Delete(leastValue);
        node.Value = leastValue;

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

    private bool DeleteWithLeftChildNode(Node parent, Node node)
    {
        if (m_root != null && m_root.Value == node.Value)
        {
            m_root = null;
            return true;
        }

        if (parent.Value < node.Value)
        {
            parent.Right = node.Left;
        }
        else
        {
            parent.Left = node.Left;
        }

        return true;
    }

    private bool DeleteWithRightChildNode(Node parent, Node node)
    {
        if (m_root != null && m_root.Value == node.Value)
        {
            m_root = null;
            return true;
        }

        if (parent.Value < node.Value)
        {
            parent.Right = node.Right;
        }
        else
        {
            parent.Left = node.Right;
        }

        return true;
    }

    private bool DeleteWithoutChildNodes(Node parent, Node node)
    {
        if (m_root != null && m_root.Value == node.Value)
        {
            m_root = null;
            return true;
        }

        if (parent.Value < node.Value)
        {
            parent.Right = null;
        }
        else
        {
            parent.Left = null;
        }

        return true;
    }
}
