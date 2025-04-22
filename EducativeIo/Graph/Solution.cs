using System.Text;

namespace EducativeIo.Graph
{
    public partial class Solution
    {
        public static string GraphtoString(EducativeIo.Graph.Graph g)
        {
            string result = "";

            for (int i = 0; i < g.GetVertices(); i++)
            {
                LinkedList.Node temp = (g.GetArray())[i].GetHead();
                result += "|" + i.ToString() + "|=>";
                while (temp != null)
                {
                    result = result + temp.m_data.ToString() + "->";
                    temp = temp.m_nextElement;
                }

                result += "NULL ";
            }

            return result;
        }

        // public static void bfsTraversal_helper(Graph g, int source, bool[] visited, ref string result)
        // {

        //     if (g.getVertices() < 1)
        //     {
        //         return;
        //     }
        //     //Create Queue(Implemented in previous chapters) for Breadth First Traversal and enqueue source in it
        //     //myQueue queue(g.getVertices());
        //     Queue<int> queue = new Queue<int> { };

        //     queue.Enqueue(source);
        //     visited[source] = true;
        //     int current_node;
        //     //Traverse while queue is not empty
        //     while (queue.Count != 0)
        //     {

        //         //Dequeue a vertex/node from queue and add it to result
        //         current_node = queue.Dequeue();

        //         result += current_node.ToString();

        //         //Get adjacent vertices to the current_node from the array,
        //         //and if they are not already visited then enqueue them in the Queue
        //         LinkedList.Node temp = (g.getArray())[current_node].GetHead();

        //         while (temp != null)
        //         {

        //             if (!visited[temp.data])
        //             {
        //                 queue.Enqueue(temp.data);
        //                 visited[temp.data] = true; //Visit the current Node
        //             }
        //             temp = temp.nextElement;
        //         }
        //     } //end of while
        // }
        // public static string Bfs(Graph g)
        // {
        //     string result = "";

        //     bool[] visited = new bool[g.getVertices()];
        //     for (int i = 0; i < g.getVertices(); i++)
        //     {
        //         if (!visited[i])
        //         {
        //             bfsTraversal_helper(g, i, visited, ref result);
        //         }
        //     }

        //     return result;
        // }


        public static string Bfs(EducativeIo.Graph.Graph graph)
        {
            if (graph.GetVertices() < 1)
                return "";

            StringBuilder result = new StringBuilder();
            bool[] visited = new bool[graph.GetVertices()];

            for (int i = 0; i < graph.GetVertices(); i++)
            {
                if (visited[i])
                    continue;

                Queue<int> queue = new Queue<int>();
                queue.Enqueue(i);
                visited[i] = true;

                while (queue.Count > 0)
                {
                    int currentNode = queue.Dequeue();
                    result.Append(currentNode.ToString() + " ");

                    LinkedList.Node adjacentNodes = graph.GetArray()[currentNode].GetHead();
                    while (adjacentNodes != null)
                    {
                        if (!visited[adjacentNodes.m_data])
                        {
                            queue.Enqueue(adjacentNodes.m_data);
                            visited[adjacentNodes.m_data] = true;
                        }

                        adjacentNodes = adjacentNodes.m_nextElement;
                    }
                }
            }

            return result.ToString();
        }

        public static string Dfs(EducativeIo.Graph.Graph graph)
        {
            if (graph.GetVertices() < 1)
                return "";

            StringBuilder result = new StringBuilder();
            bool[] visited = new bool[graph.GetVertices()];

            for (int i = 0; i < graph.GetVertices(); i++)
            {
                if (visited[i])
                    continue;
                Stack<int> stack = new Stack<int>();
                stack.Push(i);
                visited[i] = true;

                while (stack.Count > 0)
                {
                    int currentNode = stack.Pop();
                    result.Append(currentNode.ToString() + " ");

                    LinkedList.Node adjacentNodes = graph.GetArray()[currentNode].GetHead();
                    while (adjacentNodes != null)
                    {
                        if (!visited[adjacentNodes.m_data])
                        {
                            stack.Push(adjacentNodes.m_data);
                            visited[adjacentNodes.m_data] = true;
                        }

                        adjacentNodes = adjacentNodes.m_nextElement;
                    }
                }
            }

            return result.ToString();
        }
    }
}
