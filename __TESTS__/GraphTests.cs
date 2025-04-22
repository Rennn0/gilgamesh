using EducativeIo.Graph;

namespace __TESTS__;

[TestClass]
public class GraphTests
{
    [TestMethod]
    public void Graph_HasCycle()
    {
        Graph g = new Graph(3);
        g.AddEdge(0, 1);
        g.AddEdge(1, 2);
        g.AddEdge(2, 0);
        Assert.IsTrue(CycleInGraph.DetectCycle(g));
    }

    [TestMethod]
    public void Graph_HasCycle1()
    {
        Graph g = new Graph(7);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 4);
        g.AddEdge(1, 3);
        g.AddEdge(1, 5);
        g.AddEdge(2, 0);
        g.AddEdge(3, 6);
        g.AddEdge(4, 2);
        Assert.IsTrue(CycleInGraph.DetectCycle(g));
    }

    [TestMethod]
    public void Graph_HasCycle2()
    {
        Graph g2 = new Graph(5);
        g2.AddEdge(0, 1);
        g2.AddEdge(1, 2);
        g2.AddEdge(2, 3);
        g2.AddEdge(3, 1);
        g2.AddEdge(3, 4);
        Assert.IsTrue(CycleInGraph.DetectCycle(g2));
    }

    [TestMethod]
    public void Graph_NoCycle()
    {
        Graph g2 = new Graph(5);
        g2.AddEdge(0, 1);
        g2.AddEdge(1, 2);
        g2.AddEdge(2, 3);
        g2.AddEdge(2, 4);
        g2.AddEdge(3, 4);
        Assert.IsFalse(CycleInGraph.DetectCycle(g2));
    }

    [TestMethod]
    public void Graph_NewHasCycle()
    {
        Graph g = new Graph(5);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 3);
        g.AddEdge(2, 3);
        g.AddEdge(2, 1);
        g.AddEdge(3, 4);
        g.AddEdge(4, 1);
        Assert.IsTrue(CycleInGraph.DetectCycle(g));
    }

    [TestMethod]
    public void Graph_NewNoCycle()
    {
        Graph g = new Graph(4);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(2, 3);
        Assert.IsFalse(CycleInGraph.DetectCycle(g));
    }
    
}
