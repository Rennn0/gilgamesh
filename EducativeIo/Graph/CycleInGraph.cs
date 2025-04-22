namespace EducativeIo.Graph;

public class CycleInGraph
{
    public static bool DetectCycleRec(Graph g, int i, bool[] visited, bool[] recNodes)
    {
        // Check if current node is being visited in the same recursive call

        if (!visited[i])
        {
            // Set recursive array and visited to true
            visited[i] = true;
            recNodes[i] = true;

            LinkedList.Node adjacentNode = (g.GetArray())[i].GetHead();
            while (adjacentNode != null)
            {
                // Access adjacent node and repeat algorithm
                int adjacent = adjacentNode.m_data;
                if ((!visited[adjacent]) && DetectCycleRec(g, adjacent, visited, recNodes))
                    return true; // Loop found

                if (recNodes[adjacent])
                    return true;
                adjacentNode = adjacentNode.m_nextElement;
            }
        }

        recNodes[i] = false;
        return false;
    }

    public static bool DetectCycle(Graph g)
    {
        // bool[] visited = new bool[g.GetVertices()];
        // bool[] rec = new bool[g.GetVertices()];

        // for (int i = 0; i < g.GetVertices(); i++)
        // {
        //     if (DetectCycleRec(g, i, visited, rec))
        //     {
        //         return true;
        //     }
        // }

        // return false;

        int vertices = g.GetVertices();
        bool[] visited = new bool[vertices];
        bool[] recursion = new bool[vertices];
        for (int i = 0; i < vertices; i++)
        {
            if (visited[i])
                continue;

            Stack<int> stack = new Stack<int>();
            stack.Push(i);

            while (stack.Count > 0)
            {
                int current = stack.Peek();
                if (!visited[current])
                {
                    visited[current] = true;
                    recursion[current] = true;

                    LinkedList.Node adjacentNodes = g.GetArray()[current].GetHead();
                    while (adjacentNodes != null)
                    {
                        int adjacent = adjacentNodes.m_data;
                        if (!visited[adjacent])
                        {
                            stack.Push(adjacent);
                        }
                        else if (recursion[adjacent])
                        {
                            return true;
                        }

                        adjacentNodes = adjacentNodes.m_nextElement;
                    }
                }
                else
                {
                    recursion[current] = false;
                    stack.Pop();
                }
            }
        }

        return false;
    }
}
