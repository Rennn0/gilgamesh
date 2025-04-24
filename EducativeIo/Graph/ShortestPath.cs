using System.Text;

namespace EducativeIo.Graph;

public partial class Solution
{
    public static string ShortestPath(Graph g, int source, int destination)
    {
        if (source == destination)
            return string.Empty;

        Dictionary<int, int> parentChild = new Dictionary<int, int>();
        Queue<int> queue = new Queue<int>();
        bool[] visited = new bool[g.GetVertices()];

        queue.Enqueue(source);
        parentChild[source] = -1;

        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            if (node == destination)
                return ReconstructPath(parentChild, source, destination);

            LinkedList.Node adj = g.GetArray()[node].GetHead();
            while (adj != null)
            {
                if (!visited[adj.m_data])
                {
                    visited[adj.m_data] = true;
                    parentChild[adj.m_data] = node;
                    queue.Enqueue(adj.m_data);
                }

                adj = adj.m_nextElement;
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
