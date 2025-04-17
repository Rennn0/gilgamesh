using EducativeIo.StackQueue;

namespace __TESTS__;

[TestClass]
public sealed class StackQueueTests
{
    [TestMethod]
    public void TestNextGreaterElementStack1()
    {
        int[] arr = [4, 5, 2, 10, 8];
        int size = arr.Length;
        int[] expected = [5, 10, 10, -1, -1];
        int[] result = Challenge7.NextGreaterElementStack(arr, size);
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestNextGreaterElementStack2()
    {
        int[] arr = [4, 6, 3, 2, 8, 1, 9, 9];
        int size = arr.Length;
        int[] expected = [6, 8, 8, 8, 9, 9, -1, -1];
        int[] result = Challenge7.NextGreaterElementStack(arr, size);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestNextGreaterElementBruteForce3()
    {
        int[] arr = [4, 5, 2, 10, 8];
        int size = arr.Length;
        int[] expected = [5, 10, 10, -1, -1];
        int[] result = Challenge7.NextGreaterElementBruteForce(arr, size);
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestNextGreaterElementBruteForce4()
    {
        int[] arr = [4, 6, 3, 2, 8, 1, 9, 9];
        int size = arr.Length;
        int[] expected = [6, 8, 8, 8, 9, 9, -1, -1];
        int[] result = Challenge7.NextGreaterElementBruteForce(arr, size);

        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsBalanced_Ok()
    {
        const string exp = "{[()]}";
        const bool expected = true;
        bool result = Challenge8.IsBalanced(exp);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsBalanced_Bad()
    {
        const string exp = "{[(])}";
        const bool expected = false;
        bool result = Challenge8.IsBalanced(exp);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsBalanced_OkLong()
    {
        const string exp = "{{[[(())]]}}";
        const bool expected = true;
        bool result = Challenge8.IsBalanced(exp);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsBalanced_BadShort()
    {
        const string exp = "{[}";
        const bool expected = false;
        bool result = Challenge8.IsBalanced(exp);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsNewStack_MultiplePops()
    {
        NewStack newStack = new NewStack(5);
        newStack.Push(9);
        newStack.Push(3);
        newStack.Push(1);

        int expected = 1;
        int result = newStack.Min();
        Assert.AreEqual(expected, result);

        newStack.Pop();
        expected = 3;
        result = newStack.Min();
        Assert.AreEqual(expected, result);

        newStack.Push(2);
        expected = 2;
        result = newStack.Min();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsNewStack_Basic()
    {
        NewStack newStack = new NewStack(6);
        newStack.Push(9);
        newStack.Push(3);
        newStack.Push(1);
        newStack.Push(4);
        newStack.Push(2);
        newStack.Push(5);
        const int expected = 1;
        int result = newStack.Min();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsNewStack_DoubleMin()
    {
        NewStack newStack = new NewStack(6);
        newStack.Push(9);
        newStack.Push(3);
        newStack.Push(1);
        newStack.Push(4);
        newStack.Push(2);
        newStack.Push(1);

        newStack.Pop();
        newStack.Pop();

        const int expected = 1;
        int result = newStack.Min();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsPostfix1()
    {
        const string exp = "9,4,2,+,*,6,14,7,/,+,*";
        const int expected = 432;
        int result = Challenge6.EvaluatePostFix(exp);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestIsPostfix2()
    {
        const string exp = "2,3,1,*,+,9,-";
        const int expected = -4;
        int result = Challenge6.EvaluatePostFix(exp);
        Assert.AreEqual(expected, result);
    }
}
