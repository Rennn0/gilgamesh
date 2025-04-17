using chapter_5;

public class CycleInGraph
{
    public static bool detectCycleRec(Graph g, int i, bool[] visited, bool[] recNodes)
    {
        // Check if current node is being visited in the same recursive call

        if (visited[i] == false)
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

        // Write your code here
        return false;
    }
}