namespace EducativeIo.Graph;

public partial class Solution
{
    // NOTE https://youcademy.org/graph-shortest-path-using-bfs/  java implementation
    public static int FindMin(Graph g, int source, int destination)
    {
        if (source == destination)
            return 0;

        Queue<int> currentLayer = new Queue<int>();
        Queue<int> nextLayer = new Queue<int>();

        int distance = 0;
        int vertices = g.GetVertices();
        bool[] visited = new bool[vertices];

        visited[source] = true;
        currentLayer.Enqueue(source);

        while (currentLayer.Count > 0)
        {
            int layerSize = currentLayer.Count;
            for (int i = 0; i < layerSize; i++)
            {
                int node = currentLayer.Dequeue();
                if (node == destination)
                {
                    return distance;
                }

                LinkedList.Node adjacentNodes = g.GetArray()[node].GetHead();
                while (adjacentNodes != null)
                {
                    if (!visited[adjacentNodes.m_data])
                    {
                        visited[adjacentNodes.m_data] = true;
                        nextLayer.Enqueue(adjacentNodes.m_data);
                    }

                    adjacentNodes = adjacentNodes.m_nextElement;
                }
            }

            currentLayer = nextLayer;
            nextLayer = new Queue<int>();
            distance++;
        }

        return -1;
    }

    public static int FindMinWithArr(Graph g, int source, int destination)
    {
        if (source == destination)
            return 0;
        int vertices = g.GetVertices();

        bool[] visited = new bool[vertices];
        int[] distance = Enumerable.Repeat(-1, vertices).ToArray();
        Queue<int> queue = new Queue<int>();

        queue.Enqueue(source);
        visited[source] = true;
        distance[source] = 0;

        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            LinkedList.Node adjacentNodes = g.GetArray()[node].GetHead();
            while (adjacentNodes != null)
            {
                if (!visited[adjacentNodes.m_data])
                {
                    distance[adjacentNodes.m_data] = distance[node] + 1;
                    queue.Enqueue(adjacentNodes.m_data);
                    visited[adjacentNodes.m_data] = true;

                    //if (adjacentNodes.m_data == destination)
                    //{
                    //    return distance[adjacentNodes.m_data];
                    //}
                }

                adjacentNodes = adjacentNodes.m_nextElement;
            }
        }

        return distance[destination];
    }
}
