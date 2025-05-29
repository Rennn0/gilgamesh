namespace EducativeIo.Graph;

public partial class Solution
{
    public static void Dfs(Graph g, int node, bool[] visited)
    {
        visited[node] = true;

        LinkedList.Node temp = (g.GetArray())[node].GetHead();
        while (temp != null)
        {
            if (visited[temp.MData] == false)
                Dfs(g, temp.MData, visited);
            temp = temp.MNextElement;
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
            DoDfs(g, i, visited);
            bool isMother = visited.All(v => v);
            if (isMother)
                return i;
        }

        return -1;
    }

    private static void DoBfs(Graph g, int source, bool[] visited)
    {
        Queue<int> q = new();
        q.Enqueue(source);
        visited[source] = true;

        while (q.Count > 0)
        {
            int current = q.Dequeue();
            LinkedList.Node adjacents = g.GetArray()[current].GetHead();
            while (adjacents != null)
            {
                if (!visited[adjacents.MData])
                {
                    q.Enqueue(adjacents.MData);
                    visited[adjacents.MData] = true;
                }

                adjacents = adjacents.MNextElement;
            }
        }
    }

    private static void DoDfs(Graph g, int source, bool[] visited)
    {
        Stack<int> stack = new Stack<int>();
        stack.Push(source);
        visited[source] = true;

        while (stack.Count > 0)
        {
            int current = stack.Pop();
            LinkedList.Node adjacents = g.GetArray()[current].GetHead();
            while (adjacents != null)
            {
                if (!visited[adjacents.MData])
                {
                    stack.Push(adjacents.MData);
                    visited[adjacents.MData] = true;
                }

                adjacents = adjacents.MNextElement;
            }
        }
    }

    public static int FindMotherVertexLastFinishedVertex(Graph g)
    {
        bool[] visited = new bool[g.GetVertices()];
        int lastVisited = -1;
        for (int i = 0; i < g.GetVertices(); i++)
        {
            if (visited[i])
                continue;

            DoBfs(g, i, visited);
            lastVisited = i;
        }

        visited = new bool[g.GetVertices()];
        DoDfs(g, lastVisited, visited);
        bool isMother = visited.All(v => v);

        return isMother ? lastVisited : -1;
    }

    public static int NumEdges(Graph g) => g.GetArray().Sum(adj => adj.Length()) / 2;
}
