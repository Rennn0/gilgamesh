namespace EducativeIo.Bst;

public class Donk
{
    public int D { get; set; }
}

public ref struct DonkStruct
{
    public int D { get; set; }
}

public class Tree
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

    public void Arr(ref int[] arr)
    {
        arr = new int[10];
        arr[0] = 10;
    }

    public void Arr(int[] arr)
    {
        arr = new int[100];
        arr[0] = 10;
    }

    public void Arr(List<int> arr)
    {
        arr[0] *= 2;
    }

    public void Arr(ref List<int> arr)
    {
        arr[0] *= 2;
    }

    public void Arr(Donk donk)
    {
        donk.D *= 2;
    }

    public void Arr(DonkStruct donk)
    {
        donk.D *= 2;
    }

    public void Arr(ref DonkStruct donk)
    {
        donk.D *= 2;
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

    public void InorderPrint(Node? node)
    {
        if (node == null)
            return;
        InorderPrint(node.Left);
        Console.Write($"{node.Value} ");
        InorderPrint(node.Right);
    }
}
