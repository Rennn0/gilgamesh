namespace EducativeIo.Graph;

public partial class Solution
{
    public static bool CheckPath(Graph g, int source, int destination)
    {
        if (source == destination)
            return true;

        Stack<int> stack = new Stack<int>();
        bool[] visited = new bool[g.GetVertices()];

        stack.Push(source);
        visited[source] = true;

        while (stack.Count > 0)
        {
            int node = stack.Pop();
            LinkedList.Node adjNodes = g.GetArray()[node].GetHead();
            while (adjNodes != null)
            {
                if (adjNodes.m_data == destination)
                    return true;

                if (!visited[adjNodes.m_data])
                {
                    stack.Push(adjNodes.m_data);
                    visited[adjNodes.m_data] = true;
                }

                adjNodes = adjNodes.m_nextElement;
            }
        }

        return false;
    }
}
