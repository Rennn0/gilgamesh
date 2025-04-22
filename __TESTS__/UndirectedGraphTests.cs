using EducativeIo.Graph;

namespace __TESTS__;

[TestClass]
public class UndirectedGraphTests
{
    [TestMethod]
    public void Graph_NumEdges()
    {
        Graph g = new Graph(5, false);
        g.AddEdge(2, 0);
        g.AddEdge(2, 1);
        g.AddEdge(2, 3);
        g.AddEdge(2, 4);
        g.AddEdge(0, 1);
        Assert.AreEqual(5, Solution.NumEdges(g));
    }

    [TestMethod]
    public void Graph_NumEdges1()
    {
        Graph g = new Graph(5, false);
        g.AddEdge(2, 0);
        g.AddEdge(2, 1);
        g.AddEdge(2, 3);
        g.AddEdge(2, 4);
        Assert.AreEqual(4, Solution.NumEdges(g));
    }

    [TestMethod]
    public void Graph_NumEdges2()
    {
        Graph g = new Graph(8, false);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 3);
        g.AddEdge(2, 3);
        g.AddEdge(2, 4);
        g.AddEdge(3, 5);
        g.AddEdge(4, 5);
        g.AddEdge(1, 5);
        g.AddEdge(5, 6);
        g.AddEdge(5, 7);
        g.AddEdge(6, 7);

        Assert.AreEqual(11, Solution.NumEdges(g));
    }

    [TestMethod]
    public void Graph_IsTree()
    {
        Graph g = new Graph(4, false);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(0, 3);
        g.AddEdge(3, 4);

        Assert.IsTrue(Solution.IsTree(g));
    }

    [TestMethod]
    public void Graph_IsTree1()
    {
        Graph g = new Graph(4, false);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(0, 3);
        g.AddEdge(2, 3);

        Assert.IsFalse(Solution.IsTree(g));
    }
}
