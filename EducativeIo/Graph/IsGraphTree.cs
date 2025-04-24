namespace EducativeIo.Graph;

public partial class Solution
{
    public static bool HasCycle(Graph g, int node, int parent, ref bool[] visited)
    {
        visited[node] = true;
        LinkedList.Node adj = g.GetArray()[node].GetHead();
        while (adj != null)
        {
            if (!visited[adj.m_data])
            {
                if (HasCycle(g, adj.m_data, node, ref visited))
                {
                    return true;
                }
            }
            else if (adj.m_data != parent)
            {
                return true;
            }

            adj = adj.m_nextElement;
        }

        return false;
    }

    public static bool IsTree(Graph g)
    {
        bool[] visited = new bool[g.GetVertices()];
        int vertices = g.GetVertices();

        int edges = g.GetArray().Sum(l => l.Length()) / 2;
        return (edges >= vertices - 1) && !HasCycle(g, 0, -1, ref visited);
    }
}
