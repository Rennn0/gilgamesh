namespace EducativeIo.Bst;

public partial class Tree
{
    public void RotateLeftLeft(int val)
    {
        FindParentChild(val, out Node? parent, out Node? child);

        if (child is null || parent is null)
            return;

        LeftLeft(child, parent);
    }

    public void RotateRightRight(int val)
    {
        FindParentChild(val, out Node? parent, out Node? child);

        if (child is null || parent is null)
            return;

        RightRight(child, parent);
    }

    private void FindParentChild(int val, out Node? parent, out Node? child)
    {
        parent = m_root;
        child = m_root;

        while (child != null && child.Value != val)
        {
            parent = child;
            child = val < child.Value ? child.Left : child.Right;
        }
    }

    private Node? FindParent(int val)
    {
        Node? parent = m_root;
        Node? child = m_root;
        while (child != null && child.Value != val)
        {
            parent = child;
            child = val < child.Value ? child.Left : child.Right;
        }

        return parent;
    }

    private void LeftLeft(Node child, Node parent)
    {
        Node? grandParent = FindParent(parent.Value);
        parent.Left = child.Right;
        child.Right = parent;

        if (parent == m_root)
        {
            m_root = child;
        }
        else if (grandParent is not null)
        {
            if (grandParent.Left == parent)
            {
                grandParent.Left = child;
            }
            else
            {
                grandParent.Right = child;
            }
        }
    }

    private void RightRight(Node child, Node parent)
    {
        Node? grandParent = FindParent(parent.Value);
        parent.Right = child.Left;
        child.Left = parent;

        if (parent == m_root)
        {
            m_root = child;
        }
        else if (grandParent is not null)
        {
            if (grandParent.Left == parent)
            {
                grandParent.Left = child;
            }
            else
            {
                grandParent.Right = child;
            }
        }
    }
}
