using EducativeIo.Graph;

namespace __TESTS__;

[TestClass]
public class DirectedGraphTests
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

    [TestMethod]
    public void Graph_MotherVertex()
    {
        Graph g = new Graph(7);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 3);
        g.AddEdge(1, 4);
        g.AddEdge(2, 5);
        g.AddEdge(2, 6);
        Assert.AreEqual(0, Solution.FindMotherVertexDfs(g));
    }

    [TestMethod]
    public void Graph_MotherVertex1()
    {
        Graph g = new Graph(4);
        g.AddEdge(0, 1);
        g.AddEdge(1, 2);
        g.AddEdge(3, 0);
        g.AddEdge(3, 1);
        Assert.AreEqual(3, Solution.FindMotherVertexDfs(g));
    }

    [TestMethod]
    public void Graph_MotherVertex2()
    {
        Graph g = new Graph(4);
        g.AddEdge(0, 1);
        g.AddEdge(1, 2);
        g.AddEdge(3, 2);
        Assert.AreEqual(-1, Solution.FindMotherVertexDfs(g));
    }

    [TestMethod]
    public void Graph_MotherVertexLFI()
    {
        Graph g = new Graph(7);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 3);
        g.AddEdge(1, 4);
        g.AddEdge(2, 5);
        g.AddEdge(2, 6);
        Assert.AreEqual(0, Solution.FindMotherVertexLastFinishedVertex(g));
    }

    [TestMethod]
    public void Graph_MotherVertexLFI1()
    {
        Graph g = new Graph(4);
        g.AddEdge(0, 1);
        g.AddEdge(1, 2);
        g.AddEdge(3, 0);
        g.AddEdge(3, 1);
        Assert.AreEqual(3, Solution.FindMotherVertexLastFinishedVertex(g));
    }

    [TestMethod]
    public void Graph_MotherVertexLFI2()
    {
        Graph g = new Graph(4);
        g.AddEdge(0, 1);
        g.AddEdge(1, 2);
        g.AddEdge(3, 2);
        Assert.AreEqual(-1, Solution.FindMotherVertexLastFinishedVertex(g));
    }

    [TestMethod]
    public void Graph_PathExists()
    {
        Graph g = new Graph(8);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 3);
        g.AddEdge(1, 6);
        g.AddEdge(2, 3);
        g.AddEdge(2, 4);
        g.AddEdge(3, 6);
        g.AddEdge(6, 4);
        g.AddEdge(6, 5);
        g.AddEdge(6, 7);
        g.AddEdge(7, 5);
        Assert.IsTrue(Solution.CheckPath(g, 0, 7));
    }

    [TestMethod]
    public void Graph_PathExists1()
    {
        Graph g = new Graph(8);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(1, 3);
        g.AddEdge(1, 6);
        g.AddEdge(2, 3);
        g.AddEdge(2, 4);
        g.AddEdge(3, 6);
        g.AddEdge(6, 4);
        g.AddEdge(6, 5);
        // g.AddEdge(6, 7);
        g.AddEdge(7, 5);
        Assert.IsFalse(Solution.CheckPath(g, 0, 7));
    }

    [TestMethod]
    public void Graph_ShortestDistance()
    {
        Graph g = new Graph(6);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(0, 3);
        g.AddEdge(2, 4);
        g.AddEdge(3, 5);
        g.AddEdge(5, 4);
        Assert.AreEqual(2, Solution.FindMin(g, 0, 4));
    }

    [TestMethod]
    public void Graph_ShortestDistanceArray()
    {
        Graph g = new Graph(7);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(0, 3);
        g.AddEdge(1, 2);
        g.AddEdge(2, 4);
        g.AddEdge(2, 3);
        g.AddEdge(3, 5);
        g.AddEdge(5, 4);
        g.AddEdge(5, 6);
        Assert.AreEqual(2, Solution.FindMinWithArr(g, 0, 4));
    }

    [TestMethod]
    public void Graph_ShortestDistanceArray2()
    {
        Graph g = new Graph(7);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(0, 3);
        g.AddEdge(1, 2);
        g.AddEdge(2, 4);
        g.AddEdge(2, 3);
        g.AddEdge(3, 5);
        g.AddEdge(5, 4);
        g.AddEdge(5, 6);
        Assert.AreEqual(4, Solution.FindMinWithArr(g, 1, 6));
    }

    [TestMethod]
    public void Graph_ShortestPath()
    {
        Graph g = new Graph(6);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(0, 3);
        g.AddEdge(2, 4);
        g.AddEdge(3, 5);
        g.AddEdge(5, 4);

        Assert.AreEqual("0 2 4", Solution.ShortestPath(g, 0, 4));
    }

    [TestMethod]
    public void Graph_ShortestPath1()
    {
        Graph g = new Graph(7);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(0, 3);
        g.AddEdge(1, 2);
        g.AddEdge(2, 4);
        g.AddEdge(2, 3);
        g.AddEdge(3, 5);
        g.AddEdge(5, 4);
        g.AddEdge(5, 6);
        Assert.AreEqual("1 2 3 5 6", Solution.ShortestPath(g, 1, 6));
    }

    [TestMethod]
    public void Graph_ShortestPath2()
    {
        Graph g = new Graph(8);
        g.AddEdge(0, 1);
        g.AddEdge(0, 2);
        g.AddEdge(0, 3);
        g.AddEdge(1, 4);
        // g.AddEdge(2, 5);
        g.AddEdge(2, 6);
        g.AddEdge(3, 6);
        g.AddEdge(2, 2);
        g.AddEdge(5, 4);
        g.AddEdge(5, 7);
        g.AddEdge(6, 5);

        Assert.AreEqual("0 3 6 5 7", Solution.ShortestPath(g, 0, 7));
    }
}
