using chapter_5;

public class CycleInGraph
{
    public static bool detectCycleRec(Graph g, int i, bool[] visited, bool[] recNodes)
    {
        // Check if current node is being visited in the same recursive call

        if (!visited[i])
        {
            // Set recursive array and visited to true
            visited[i] = true;
            recNodes[i] = true;

            int adjacent;
            LinkedList.Node adjacentNode = (g.getArray())[i].GetHead();
            while (adjacentNode != null)
            {
                // Access adjacent node and repeat algorithm
                adjacent = adjacentNode.data;
                if ((!visited[adjacent]) && detectCycleRec(g, adjacent, visited, recNodes))
                    return true;  // Loop found
                else if (recNodes[adjacent])
                    return true;
                adjacentNode = adjacentNode.nextElement;
            }

        }
        recNodes[i] = false;
        return false;
    }
    public static bool detectCycle(Graph g)
    {
        bool[] visited = new bool[g.getVertices()];
        bool[] rec = new bool[g.getVertices()];

        for (int i = 0; i < g.getVertices(); i++)
        {
            if (detectCycleRec(g, i, visited, rec))
            {
                return true;
            }
        }
        return false;
    }
}