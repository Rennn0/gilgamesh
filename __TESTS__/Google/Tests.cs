using EducativeIo.Projects.Google;

namespace __TESTS__.Google
{
    [TestClass]
    public class GoogleTests
    {
        [TestMethod]
        public void MaximumSubarraySum()
        {
            Feature feature = new Feature();
            int a1 = feature.MaximumSubarraySum([2, 3, -8, 7, -1, 2, 3]);
            int a2 = feature.MaximumSubarraySum([-2, -3]);
            Assert.AreEqual(11, a1);
            Assert.AreEqual(-2, a2);
        }

        [TestMethod]
        public void BinaryNums()
        {
            Feature feature = new Feature();
            List<string> nums = feature.BinaryNums(7);
        }

        [TestMethod]
        public void SortStack()
        {
            Feature feature = new Feature();
            Stack<int> stack = new Stack<int>();
            stack.Push(3);
            stack.Push(1);
            stack.Push(5);
            stack.Push(4);
            stack.Push(10);
            stack.Push(0);
            feature.SortStack(stack);
            Assert.AreEqual(0, stack.Pop());
            Assert.AreEqual(1, stack.Pop());
            Assert.AreEqual(3, stack.Pop());
            Assert.AreEqual(4, stack.Pop());
            Assert.AreEqual(5, stack.Pop());
            Assert.AreEqual(10, stack.Pop());
        }

        [TestMethod]
        public void Islands()
        {
            Feature feature = new Feature();
            int[][] matrix =
             [
                [1,1,0,0,0],
                [1,1,0,0,0],
                [0,0,1,0,0],
                [0,0,0,1,1],
            ];

            Assert.AreEqual(3, feature.Islands(matrix));
        }

        [TestMethod]
        public void RotateLeft()
        {
            Feature feature = new Feature();
            int[] arr = [1, 2, 3, 4, 5];
            feature.RotateLeft(arr, 2);
            CollectionAssert.AreEquivalent(new int[] { 3, 4, 5, 1, 2 }, arr);

            int[] arr2 = [1, 2, 3, 4, 5, 6, 7, 8];
            feature.RotateLeft(arr2, 3);
            CollectionAssert.AreEquivalent(new int[] { 4, 5, 6, 7, 8, 1, 2, 3 }, arr2);
        }

        [TestMethod]
        public void RotateRight()
        {
            Feature feature = new Feature();
            int[] arr = [1, 2, 3, 4, 5];
            feature.RotateRight(arr, 2);
            CollectionAssert.AreEquivalent(new int[] { 4, 5, 1, 2, 3 }, arr);

            int[] arr2 = [1, 2, 3, 4, 5, 6, 7, 8];
            feature.RotateRight(arr2, 3);
            CollectionAssert.AreEquivalent(new int[] { 6, 7, 8, 1, 2, 3, 4, 5 }, arr2);
        }

        [TestMethod]
        public void MergeSort()
        {
            Feature feature = new Feature();
            int[] arr = [5, 1, 0, 2, 4, 3, 10, 3];
            feature.MergeSort(arr, 0, arr.Length - 1);

            int[] arr2 = [-1, 0, 0, -1, 5, 4, -5];
            feature.MergeSort(arr2, 0, arr2.Length - 1);

            CollectionAssert.AreEquivalent(new int[] { 0, 1, 2, 3, 3, 4, 5, 10 }, arr);
            CollectionAssert.AreEquivalent(new int[] { -5, -1, -1, 0, 0, 4, 5 }, arr2);
        }
    }
}