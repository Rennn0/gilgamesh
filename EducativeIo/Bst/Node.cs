namespace EducativeIo.Bst;

public class Node
{
    public int Value { get; set; }
    public Guid Guid { get; set; }
    public Node? Left { get; set; }
    public Node? Right { get; set; }

    public Node(int value)
    {
        Value = value;
        Guid = Guid.NewGuid();
        Left = null;
        Right = null;

        Console.WriteLine($"Node: {value}, Guid: {Guid}");
    }
}
