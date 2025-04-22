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

    public static int FindMotherVertex(Graph g)
    {
        for (int i = 0; i < g.GetVertices(); i++)
        {
            // bool[] visited = new bool[g.GetVertices()];
            // Dfs(g, i, visited);
            // bool isMother = visited.All(v => v);
            // if (isMother)
            //     return i;

            Stack<int> stack = new Stack<int>();
            bool[] visited = new bool[g.GetVertices()];
            stack.Push(i);
            visited[i] = true;

            while (stack.Count > 0)
            {
                int current = stack.Pop();
                LinkedList.Node adjList = g.GetArray()[current].GetHead();
                while (adjList != null)
                {
                    if (!visited[adjList.m_data])
                    {
                        stack.Push(adjList.m_data);
                        visited[adjList.m_data] = true;
                    }

                    adjList = adjList.m_nextElement;
                }
            }

            bool isMother = visited.All(v => v);
            if (isMother)
                return i;
        }

        return -1;
    }
}
