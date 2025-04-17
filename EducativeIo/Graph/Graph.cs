namespace EducativeIo.Graph
{
    public class Graph
    {
        private readonly int _vertices;
        private readonly LinkedList[] _array;

        public Graph(int v)
        {
            _array = new LinkedList[v];
            _vertices = v;
            for (int i = 0; i < v; i++)
            {
                _array[i] = new LinkedList();
            }
        }

        public void AddEdge(int source, int destination)
        {
            if (source < _vertices && destination < _vertices)
                _array[source].InsertAtHead(destination);
        }

        public void PrintGraph()
        {
            Console.WriteLine("Adjacency List of Directed Graph");
            for (int i = 0; i < _vertices; i++)
            {
                Console.Write("|" + i + "| => ");
                LinkedList.Node temp = (_array[i]).GetHead();

                while (temp != null)
                {
                    Console.Write("[" + temp._data + "] -> ");
                    temp = temp._nextElement;
                }

                Console.WriteLine("NULL");
            }
        }

        public LinkedList[] GetArray()
        {
            return _array;
        }

        public int GetVertices()
        {
            return _vertices;
        }
    }
}
