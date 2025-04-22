namespace EducativeIo.Graph
{
    public class Graph
    {
        private readonly int m_vertices;
        private readonly LinkedList[] m_array;

        public Graph(int v)
        {
            m_array = new LinkedList[v];
            m_vertices = v;
            for (int i = 0; i < v; i++)
            {
                m_array[i] = new LinkedList();
            }
        }

        public void AddEdge(int source, int destination)
        {
            if (source < m_vertices && destination < m_vertices)
                m_array[source].InsertAtHead(destination);
        }

        public void PrintGraph()
        {
            Console.WriteLine("Adjacency List of Directed Graph");
            for (int i = 0; i < m_vertices; i++)
            {
                Console.Write("|" + i + "| => ");
                LinkedList.Node temp = (m_array[i]).GetHead();

                while (temp != null)
                {
                    Console.Write("[" + temp.m_data + "] -> ");
                    temp = temp.m_nextElement;
                }

                Console.WriteLine("NULL");
            }
        }

        public LinkedList[] GetArray()
        {
            return m_array;
        }

        public int GetVertices()
        {
            return m_vertices;
        }
    }
}