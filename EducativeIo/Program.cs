namespace EducativeIo;

internal class Solution
{
    public static void Main(string[] args)
    {
        EducativeIo.Graph.Graph g = new Graph.Graph(7);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 4);
        g.AddEdge(1, 3);
        g.AddEdge(1, 5);
        g.AddEdge(3, 6);

        g.PrintGraph();
        Console.WriteLine(Graph.Solution.Bfs(g));
        Console.WriteLine(Graph.Solution.Dfs(g));
    }
}
