using System.Text;
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

    [TestMethod]
    public void Preorder()
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
        StringBuilder sb = new StringBuilder();
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("5 1 0 2 3 9 17 14 24", sb.ToString());
    }

    [TestMethod]
    public void Postorder()
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
        StringBuilder sb = new StringBuilder();
        tree.PostOrder(tree.GetRoot(), ref sb);
        sb.Length--;

        Assert.AreEqual("0 3 2 1 14 24 17 9 5", sb.ToString());
    }

    [TestMethod]
    public void Height()
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

        Assert.AreEqual(3, tree.Height());
    }

    [TestMethod]
    public void Height_()
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

        Assert.AreEqual(3, tree.Height_());
    }
}
