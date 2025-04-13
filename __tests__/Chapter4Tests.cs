using EducativeIo.Chapter4;

namespace __TESTS__;

[TestClass]
public sealed class Chapter4Tests
{
    [TestMethod]
    public void TestNextGreaterElementStack1()
    {
        int[] arr = [4, 5, 2, 10, 8];
        int size = arr.Length;
        int[] expected = [5, 10, 10, -1, -1];
        int[] result = Challenge7.nextGreaterElementStack(arr, size);
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestNextGreaterElementStack2()
    {
        int[] arr = [4, 6, 3, 2, 8, 1, 9, 9];
        int size = arr.Length;
        int[] expected = [6, 8, 8, 8, 9, 9, -1, -1];
        int[] result = Challenge7.nextGreaterElementStack(arr, size);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestNextGreaterElementBruteForce3()
    {
        int[] arr = [4, 5, 2, 10, 8];
        int size = arr.Length;
        int[] expected = [5, 10, 10, -1, -1];
        int[] result = Challenge7.nextGreaterElementBruteForce(arr, size);
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestNextGreaterElementBruteForce4()
    {
        int[] arr = [4, 6, 3, 2, 8, 1, 9, 9];
        int size = arr.Length;
        int[] expected = [6, 8, 8, 8, 9, 9, -1, -1];
        int[] result = Challenge7.nextGreaterElementBruteForce(arr, size);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsBalanced_Ok()
    {
        string exp = "{[()]}";
        bool expected = true;
        bool result = Challenge8.isBalanced(exp);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsBalanced_Bad()
    {
        string exp = "{[(])}";
        bool expected = false;
        bool result = Challenge8.isBalanced(exp);
        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public void TestIsBalanced_OkLong()
    {
        string exp = "{{[[(())]]}}";
        bool expected = true;
        bool result = Challenge8.isBalanced(exp);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsBalanced_BadShort()
    {
        string exp = "{[}";
        bool expected = false;
        bool result = Challenge8.isBalanced(exp);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsNewStack_MultiplePops()
    {
        newStack newStack = new newStack(5);
        newStack.push(9);
        newStack.push(3);
        newStack.push(1);

        int expected = 1;
        int result = newStack.min();
        Assert.AreEqual(expected, result);

        newStack.pop();
        expected = 3;
        result = newStack.min();
        Assert.AreEqual(expected, result);

        newStack.push(2);
        expected = 2;
        result = newStack.min();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsNewStack_Basic()
    {
        newStack newStack = new newStack(6);
        newStack.push(9);
        newStack.push(3);
        newStack.push(1);
        newStack.push(4);
        newStack.push(2);
        newStack.push(5);
        int expected = 1;
        int result = newStack.min();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsNewStack_DoubleMin()
    {
        newStack newStack = new newStack(6);
        newStack.push(9);
        newStack.push(3);
        newStack.push(1);
        newStack.push(4);
        newStack.push(2);
        newStack.push(1);

        newStack.pop();
        newStack.pop();

        int expected = 1;
        int result = newStack.min();
        Assert.AreEqual(expected, result);
    }
}
