using EducativeIo.Chapter4;

namespace __TESTS__;

[TestClass]
public sealed class Chapter4Tests
{
    [TestMethod]
    public void TestMethod1()
    {
        int[] arr = [4, 5, 2, 10, 8];
        int size = arr.Length;
        int[] expected = [5, 10, 10, -1, -1];
        int[] result = Solution.nextGreaterElement(arr, size);
        CollectionAssert.AreEqual(expected, result);
    }

    [TestMethod]
    public void TestMethod2()
    {
        int[] arr = [4, 6, 3, 2, 8, 1, 9, 9];
        int size = arr.Length;
        int[] expected = [6, 8, 8, 8, 9, 9, -1, -1];
        int[] result = Solution.nextGreaterElement(arr, size);

        CollectionAssert.AreEqual(expected, result);
    }
}
