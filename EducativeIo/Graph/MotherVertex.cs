using EducativeIo.Graph;

public class Solution
{
    public static void DFS(Graph g, int node, bool[] visited)
    {
        visited[node] = true;

        LinkedList.Node temp = (g.GetArray())[node].GetHead();
        while (temp != null)
        {
            if (visited[temp._data] == false)
                DFS(g, temp._data, visited);
            temp = temp._nextElement;
        }
    }

    public static int findMotherVertex(Graph g)
    {
        for (int i = 0; i < g.GetVertices(); i++)
        {
            bool[] visited = new bool[g.GetVertices()];
            DFS(g, i, visited);
            bool isMother = visited.All(v => v);
            if (isMother)
                return i;
        }
        return -1;
    }
}