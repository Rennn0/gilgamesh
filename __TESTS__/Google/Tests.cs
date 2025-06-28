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

        [TestMethod]
        public void MinMeetingRooms()
        {
            Feature feature = new Feature();
            int[][] meetings = [
                [2, 8], [3, 4], [3, 9], [5, 11], [8, 20], [11, 15]
            ];
            int[][] meetings2 = [
                [1, 4], [2, 5], [4, 8], [5, 6], [5, 8], [6, 7]
            ];
            Assert.AreEqual(3, feature.MinMeetingRooms(meetings));
            Assert.AreEqual(3, feature.MinMeetingRooms(meetings2));
        }


        [TestMethod]
        public void MaxRectangleSum()
        {
            Feature feature = new Feature();
            // List<List<int>> matrix = [
            //     [0, -2, -7, 0],
            //     [9,  2, -6, 2],
            //     [-4, 1, -4, 1],
            //     [-1, 8,  0, -2]
            // ];

            int[][] matrix =
            [
                [0, -2, -7, 0],
                [9, 2, -6, 2],
                [-4, 1, -4, 1],
                [-1, 8, 0, -2]
            ];


            Assert.AreEqual(15, feature.SubrectangleSum(matrix.ToArray()));
        }

        [TestMethod]
        public void MergeMeetings()
        {
            Feature feature = new Feature();
            List<List<int>> result = feature.MergeMeetings([
                [1,4],
                [2,5],
                [6,8],
                [7,9],
                [10,13]
            ]);
            List<List<int>> expected =
            [
                [1,5],
                [6,9],
                [10,13],
            ];
            CollectionAssert.AreEquivalent(expected.Select(e => string.Join(',', e)).ToList(), result.Select(r => string.Join(',', r)).ToList());
        }

        [TestMethod]
        public void CheckMeetings()
        {
            Feature feature = new Feature();
            int[][] meetingTimes = [
                [1, 3], [4, 6], [8, 10], [10, 12], [13, 15]
            ];

            int[] newMeeting = [7, 8];
            Assert.IsTrue(feature.CheckMeetings(meetingTimes, newMeeting));

            newMeeting = [9, 11];
            Assert.IsFalse(feature.CheckMeetings(meetingTimes, newMeeting));
        }

        [TestMethod]
        public void AddAndMergeMeetings()
        {
            Feature feature = new Feature();
            int[][] meetingTimes = [
            [1, 3], [4, 6], [8, 10], [10, 12], [13, 15], [16, 18]];
            int[] newMeeting = [9, 13];
            Assert.AreEqual(4, feature.AddAndMergeMeetings(meetingTimes, newMeeting).Count);
            newMeeting = [9, 20];
            Assert.AreEqual(3, feature.AddAndMergeMeetings(meetingTimes, newMeeting).Count);
        }

        [TestMethod]
        public void MeetingsIntersection()
        {
            Feature feature = new Feature();

            int[][] meetingsA = [[1, 3], [5, 6], [6, 9]];
            int[][] meetingsB = [[2, 3], [5, 7]];

            List<List<int>> intersecion = feature.MeetingsIntersection(meetingsA, meetingsB);
            Assert.AreEqual(3, intersecion.Count);
            Assert.AreEqual(2, intersecion[0][0]);
            Assert.AreEqual(3, intersecion[0][1]);
            Assert.AreEqual(5, intersecion[1][0]);
            Assert.AreEqual(6, intersecion[1][1]);
            Assert.AreEqual(6, intersecion[2][0]);
            Assert.AreEqual(7, intersecion[2][1]);
        }


        [TestMethod]
        public void MergeIntervals()
        {
            int[][] arr = [[1, 4], [2, 5], [6, 7], [7, 10], [11, 12]];
            int[][] res = new Solution().MergeIntervals(arr);

            Assert.AreEqual(3, res.Length);
        }
    }
}
