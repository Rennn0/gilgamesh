using System.Text;

namespace EducativeIo.Graph;

public partial class Solution
{
    public static string ShortestPath(Graph g, int source, int destination)
    {
        if (source == destination)
            return string.Empty;

        Dictionary<int, int> childParent = new Dictionary<int, int>();
        Queue<int> queue = new Queue<int>();
        bool[] visited = new bool[g.GetVertices()];

        queue.Enqueue(source);
        childParent[source] = -1;

        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            if (node == destination)
                return ReconstructPath(childParent, source, destination);

            LinkedList.Node adj = g.GetArray()[node].GetHead();
            while (adj != null)
            {
                if (!visited[adj.MData])
                {
                    visited[adj.MData] = true;
                    childParent[adj.MData] = node;
                    queue.Enqueue(adj.MData);
                }

                adj = adj.MNextElement;
            }
        }

        return string.Empty;
    }

    private static string ReconstructPath(Dictionary<int, int> dic, int source, int destination)
    {
        StringBuilder pathBuilder = new StringBuilder();

        string current = destination.ToString();
        while (dic.TryGetValue(int.Parse(current), out int parent))
        {
            pathBuilder.Insert(0, $"{current} ");
            current = parent.ToString();
        }

        pathBuilder.Length--;
        return pathBuilder.ToString();
    }
}
