using System.Text;

namespace EducativeIo.Bst;

public partial class Tree
{
    public void PreOrder(Node? node, ref StringBuilder builder)
    {
        if (node == null)
            return;

        builder.Append($"{node.Value} ");
        PreOrder(node.Left, ref builder);
        PreOrder(node.Right, ref builder);
    }

    public void PostOrder(Node? node, ref StringBuilder builder)
    {
        if (node == null)
            return;
        PostOrder(node.Left, ref builder);
        PostOrder(node.Right, ref builder);
        builder.Append($"{node.Value} ");
    }

    public void InOrder(Node? node, ref StringBuilder builder)
    {
        if (node == null)
            return;
        InOrder(node.Left, ref builder);
        builder.Append($"{node.Value} ");
        InOrder(node.Right, ref builder);
    }
}
