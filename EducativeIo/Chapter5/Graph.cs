using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chapter_5
{
    public class Graph
    {
        int vertices;
        LinkedList[] array;


        public Graph(int v)
        {
            array = new LinkedList[v];
            vertices = v;
            for (int i = 0; i < v; i++)
            {
                array[i] = new LinkedList();
            }
        }

        public void addEdge(int source, int destination)
        {
            if (source < vertices && destination < vertices)
                array[source].InsertAtHead(destination);
        }

        public void printGraph()
        {
            Console.WriteLine("Adjacency List of Directed Graph");
            LinkedList.Node temp;
            for (int i = 0; i < vertices; i++)
            {
                Console.Write("|" + i + "| => ");
                temp = (array[i]).GetHead();

                while (temp != null)
                {
                    Console.Write("[" + temp.data + "] -> ");
                    temp = temp.nextElement;
                }
                Console.WriteLine("NULL");
            }
        }

        public LinkedList[] getArray()
        {
            return array;
        }

        public int getVertices()
        {
            return vertices;
        }
    }
}
