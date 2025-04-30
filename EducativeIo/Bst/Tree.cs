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
}
