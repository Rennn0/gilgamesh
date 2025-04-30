using EducativeIo.Bst;

namespace __TESTS__;

[TestClass]
public class TreeTests
{
    [TestMethod]
    public void Basic()
    {
        Tree tree = new Tree();

        tree.Insert(5);
        tree.Insert(6);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(4);
        tree.Insert(14);
        tree.Insert(24);
        tree.Insert(10);

        tree.InorderPrint(tree.GetRoot());
    }

    [TestMethod]
    public void Find()
    {
        Tree tree = new Tree();

        tree.Insert(5);
        tree.Insert(6);
        tree.Insert(7);
        tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(4);
        tree.Insert(14);
        tree.Insert(24);
        tree.Insert(10);

        Assert.IsTrue(tree.Find(24) != null);
        Assert.IsTrue(tree.Find(-1) == null);
    }

    [TestMethod]
    public void Delete_NoChild()
    {
        Tree tree = new Tree();

        tree.Insert(5);
        tree.Insert(9);
        tree.Insert(17);
        tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(0);
        tree.Insert(14);
        tree.Insert(24);

        tree.Delete(0);
        Assert.IsTrue(tree.Find(0) == null);
    }

    [TestMethod]
    public void Delete_LeftChild()
    {
        Tree tree = new Tree();

        tree.Insert(5);
        tree.Insert(9);
        tree.Insert(17);
        tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(0);
        tree.Insert(14);
        // tree.Insert(24);

        tree.Delete(17);
        Assert.IsTrue(tree.Find(17) == null);
    }

    [TestMethod]
    public void Delete_RightChild()
    {
        Tree tree = new Tree();

        tree.Insert(5);
        tree.Insert(9);
        tree.Insert(17);
        tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(0);
        tree.Insert(14);
        tree.Insert(24);

        tree.Delete(9);
        Assert.IsTrue(tree.Find(9) == null);
    }

    [TestMethod]
    public void Delete_BothChild()
    {
        Tree tree = new Tree();

        tree.Insert(5);
        tree.Insert(9);
        tree.Insert(17);
        tree.Insert(1);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(0);
        tree.Insert(14);
        tree.Insert(24);

        tree.Delete(5);
        Assert.IsTrue(tree.Find(5) == null);
    }
}
