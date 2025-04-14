using chapter_5;
class Solution
{
    public static void Main(string[] args)
    {
        Graph g = new Graph(7);
        g.addEdge(0, 1);
        g.addEdge(0, 2);
        g.addEdge(1, 3);
        g.addEdge(1, 4);
        g.addEdge(1, 5);
        g.addEdge(3, 6);

        // Console.WriteLine(EducativeIo.Chapter5.Solution.graphtoString(g));
        // g.printGraph();
        Console.WriteLine(EducativeIo.Chapter5.Solution.Bfs(g));
    }
}