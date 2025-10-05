using EducativeIo.Diy;
using OperatingSystem = EducativeIo.Diy.OperatingSystem;

namespace __TESTS__;

[TestClass]
public class OperatingSystemTests
{
    [TestMethod]
    public void PossibleSpaceAllocation()
    {
        Assert.AreEqual(4, new OperatingSystem().PossibleSpaceAllocation([1, 2, 1, 2, 1], 3));
        Assert.AreEqual(4,
            new OperatingSystem().PossibleSpaceAllocation([1, 1, 2, 1, 1, 4, 4, 2], 2));
    }

    [TestMethod]
    public void GetMissingProcessId()
    {
        Assert.AreEqual(11, new OperatingSystem().GetMissingProcessId([5, 7, 9, 10, 13], 3));
        Assert.AreEqual(12, new OperatingSystem().GetMissingProcessId([5, 7, 9, 10, 13], 4));
        Assert.AreEqual(6, new OperatingSystem().GetMissingProcessId([5, 7, 9, 10, 13], 1));
    }

    [TestMethod]
    public void MaxPathSum()
    {
        OsNode<int> root = new OsNode<int>(-8);
        OsNode<int> l = new OsNode<int>(-10);
        OsNode<int> r = new OsNode<int>(-5);
        OsNode<int> ll = new OsNode<int>(-15);
        OsNode<int> lr = new OsNode<int>(-2);
        OsNode<int> rl = new OsNode<int>(-0);
        OsNode<int> rr = new OsNode<int>(-1);
        root.Left = l;
        root.Right = r;
        root.Left.Left = ll;
        root.Left.Right = lr;
        root.Right.Left = rl;
        root.Right.Right = rr;
        Assert.AreEqual(0, OsNode<int>.MaxPathSum(root));
    }
}