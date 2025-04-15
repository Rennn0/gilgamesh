using System.Security.Cryptography;
using System.Text;
using chapter_5;

namespace EducativeIo.Chapter5
{
    public class Solution
    {
        public static string graphtoString(Graph g)
        {
            string result = "";

            for (int i = 0; i < g.getVertices(); i++)
            {
                LinkedList.Node temp = (g.getArray())[i].GetHead();
                result += "|" + i.ToString() + "|=>";
                while (temp != null)
                {
                    result = result + temp.data.ToString() + "->";
                    temp = temp.nextElement;
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


        public static string Bfs(Graph graph)
        {
            if (graph.getVertices() < 1) return "";

            StringBuilder result = new StringBuilder();
            bool[] visited = new bool[graph.getVertices()];

            for (int i = 0; i < graph.getVertices(); i++)
            {
                if (visited[i]) continue;

                Queue<int> queue = new Queue<int>();
                queue.Enqueue(i);
                visited[i] = true;

                while (queue.Count > 0)
                {
                    int currentNode = queue.Dequeue();
                    result.Append(currentNode.ToString() + " ");

                    LinkedList.Node adjacentNodes = graph.getArray()[currentNode].GetHead();
                    while (adjacentNodes != null)
                    {
                        if (!visited[adjacentNodes.data])
                        {
                            queue.Enqueue(adjacentNodes.data);
                            visited[adjacentNodes.data] = true;
                        }

                        adjacentNodes = adjacentNodes.nextElement;
                    }
                }
            }

            return result.ToString();
        }
    }
}