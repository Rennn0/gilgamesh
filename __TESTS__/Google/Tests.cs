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
    }
}