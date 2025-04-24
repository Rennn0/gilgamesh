using System.Text;

namespace EducativeIo.Graph;

public partial class Solution
{
    public static string ShortestPath(Graph g, int source, int destination)
    {
        if (source == destination)
            return string.Empty;

        Dictionary<int, int> parentChild = new Dictionary<int, int>();
        Queue<int> currentLayer = new Queue<int>();
        Queue<int> nextLayer = new Queue<int>();

        currentLayer.Enqueue(source);
        parentChild[source] = -1;

        while (currentLayer.Count > 0)
        {
            int node = currentLayer.Dequeue();
            if (node == destination)
                return ReconstructPath(parentChild, source, destination);

            LinkedList.Node adj = g.GetArray()[node].GetHead();
            while (adj != null)
            {
                if (parentChild.TryAdd(adj.m_data, node))
                {
                    nextLayer.Enqueue(adj.m_data);
                }

                adj = adj.m_nextElement;
            }

            if (currentLayer.Count != 0)
                continue;

            currentLayer = nextLayer;
            nextLayer = new Queue<int>();
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
