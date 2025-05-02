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

    [TestMethod]
    public void RotateLeftLeft()
    {
        Tree tree = new Tree();

        tree.Insert(5);
        tree.Insert(6);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(0);
        tree.Insert(-1);
        tree.Insert(1);

        StringBuilder sb = new StringBuilder();
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("5 2 0 -1 1 3 6", sb.ToString());
        sb.Clear();

        tree.RotateLeftLeft(2);
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("2 0 -1 1 5 3 6", sb.ToString());
    }

    [TestMethod]
    public void RotateRightRight()
    {
        Tree tree = new Tree();

        tree.Insert(5);
        tree.Insert(2);
        tree.Insert(7);
        tree.Insert(6);
        tree.Insert(10);
        tree.Insert(8);
        tree.Insert(13);

        StringBuilder sb = new StringBuilder();
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("5 2 7 6 10 8 13", sb.ToString());
        sb.Clear();

        tree.RotateRightRight(7);
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("7 5 2 6 10 8 13", sb.ToString());

        tree.Insert(11);
        tree.Insert(15);
        tree.Insert(20);
        sb.Clear();

        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;

        Assert.AreEqual("7 5 2 6 10 8 13 11 15 20", sb.ToString());
        sb.Clear();

        tree.RotateRightRight(13);
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("7 5 2 6 13 10 8 11 15 20", sb.ToString());
    }

    [TestMethod]
    public void RotateRightLeft()
    {
        Tree tree = new Tree();
        tree.Insert(7);
        tree.Insert(4);
        tree.Insert(13);
        tree.Insert(10);
        tree.Insert(8);
        tree.Insert(11);
        tree.Insert(15);

        StringBuilder sb = new StringBuilder();
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("7 4 13 10 8 11 15", sb.ToString());

        tree.RotateRightLeft(10);
        sb.Clear();
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("10 7 4 8 13 11 15", sb.ToString());
    }

    [TestMethod]
    public void RotateLeftRight()
    {
        Tree tree = new Tree();
        tree.Insert(7);
        tree.Insert(3);
        tree.Insert(10);
        tree.Insert(5);
        tree.Insert(1);
        tree.Insert(4);
        tree.Insert(6);

        StringBuilder sb = new StringBuilder();
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("7 3 1 5 4 6 10", sb.ToString());

        tree.RotateLeftRight(5);
        sb.Clear();
        tree.PreOrder(tree.GetRoot(), ref sb);
        sb.Length--;
        Assert.AreEqual("5 3 1 4 7 6 10", sb.ToString());
    }
}
