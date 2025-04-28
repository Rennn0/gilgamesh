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

        Assert.IsTrue(tree.Find(6) != null);
        Assert.IsTrue(tree.Find(7) != null);
        Assert.IsTrue(tree.Find(1) != null);
        Assert.IsTrue(tree.Find(2) != null);
        Assert.IsTrue(tree.Find(3) != null);
        Assert.IsTrue(tree.Find(4) != null);
        Assert.IsTrue(tree.Find(14) != null);
        Assert.IsTrue(tree.Find(24) != null);
        Assert.IsTrue(tree.Find(10) != null);
        Assert.IsTrue(tree.Find(-1) == null);
    }

    [TestMethod]
    public void Test()
    {
        Tree tree = new Tree();

        int[] arr = new int[1];
        arr[0] = 1;

        tree.Arr(ref arr);
        tree.Arr(arr);

        List<int> list = new List<int>();
        list.Add(1);

        tree.Arr(ref list);
        tree.Arr(list);

        Donk d = new Donk { D = 1 };
        tree.Arr(d);

        DonkStruct ds = new DonkStruct { D = 2 };
        tree.Arr(ds);
        tree.Arr(ref ds);
    }
}
