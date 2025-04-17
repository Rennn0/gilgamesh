using EducativeIo.Graph;

namespace EducativeIo;

internal class Solution
{
    public static void Main(string[] args)
    {
        Graph.Graph g = new Graph.Graph(7);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 4);
        g.AddEdge(1, 3);
        g.AddEdge(1, 5);
        g.AddEdge(4, 1);
        g.AddEdge(3, 6);
        g.AddEdge(5, 4);

        g.PrintGraph();
        CycleInGraph.DetectCycle(g);
        // Console.WriteLine(Graph.Solution.Bfs(g));
        // Console.WriteLine(Graph.Solution.Dfs(g));
    }
}
