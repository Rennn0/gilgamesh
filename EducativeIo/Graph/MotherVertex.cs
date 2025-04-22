namespace EducativeIo.Graph;

public partial class Solution
{
    public static void Dfs(Graph g, int node, bool[] visited)
    {
        visited[node] = true;

        LinkedList.Node temp = (g.GetArray())[node].GetHead();
        while (temp != null)
        {
            if (visited[temp.m_data] == false)
                Dfs(g, temp.m_data, visited);
            temp = temp.m_nextElement;
        }
    }

    public static int FindMotherVertexDfs(Graph g)
    {
        for (int i = 0; i < g.GetVertices(); i++)
        {
            // bool[] visited = new bool[g.GetVertices()];
            // Dfs(g, i, visited);
            // bool isMother = visited.All(v => v);
            // if (isMother)
            //     return i;

            bool[] visited = new bool[g.GetVertices()];
            DoDfs(g, i, visited, out _);
            bool isMother = visited.All(v => v);
            if (isMother)
                return i;
        }

        return -1;
    }

    private static void DoDfs(Graph g, int source, bool[] visited, out int lastFinishedVertex)
    {
        Stack<int> stack = new Stack<int>();
        stack.Push(source);
        visited[source] = true;
        lastFinishedVertex = source;

        while (stack.Count > 0)
        {
            int current = stack.Pop();
            LinkedList.Node adjacents = g.GetArray()[current].GetHead();
            while (adjacents != null)
            {
                if (!visited[adjacents.m_data])
                {
                    stack.Push(adjacents.m_data);
                    visited[adjacents.m_data] = true;
                    lastFinishedVertex = adjacents.m_data;
                }

                adjacents = adjacents.m_nextElement;
            }
        }
    }

    public static int FindMotherVertexLastFinishedVertex(Graph g)
    {
        bool[] visited = new bool[g.GetVertices()];
        int lastVisited = -1;
        for (int i = 0; i < g.GetVertices(); i++)
        {
            if (!visited[i])
            {
                DoDfs(g, i, visited, out lastVisited);
            }
        }

        visited = new bool[g.GetVertices()];
        DoDfs(g, lastVisited, visited, out _);
        bool isMother = visited.All(v => v);

        return isMother ? lastVisited : -1;
    }
}
