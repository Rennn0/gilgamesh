namespace EducativeIo.Graph;

public partial class Solution
{
    public static void RemoveEdge(Graph g, int source, int destination)
    {
        LinkedList adj = g.GetArray()[source];
        adj.Delete(destination);
    }
}
