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
        OsNode<int> root = new OsNode<int>(-8)
        {
            Left = new OsNode<int>(-10)
            {
                Left = new OsNode<int>(-15), Right = new OsNode<int>(-2)
            },
            Right = new OsNode<int>(-5)
            {
                Left = new OsNode<int>(-0) { Right = new OsNode<int>(7) },
                Right = new OsNode<int>(-1)
            }
        };
        Assert.AreEqual(7, OsNode<int>.MaxPathSum(root));
        Assert.AreEqual("-8 -5 -10 -15 -2 0 -1 7", string.Join(" ",
            OsNode<int>.ZigZagTraversal(root).Select(x => string.Join(" ", x))));
        Assert.AreEqual("-8 -5 -1 7", string.Join(" ", OsNode<int>.RightSideView(root)));
        OsNode<int> flat = OsNode<int>.FlattenBinaryTree(root);
    }
}