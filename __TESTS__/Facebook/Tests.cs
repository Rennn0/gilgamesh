using EducativeIo.Projects.Facebook;

namespace __TESTS__.Facebook
{
    [TestClass]
    public class FacebookTests
    {
        [TestMethod]
        public void NumIslands()
        {
            Solution solution = new Solution();
            Assert.AreEqual(3, solution.NumIslands([
                ["1", "1", "0", "0", "0"],
                ["1", "1", "0", "0", "0"],
                ["0", "0", "1", "0", "0"],
                ["0", "0", "0", "1", "1"]
            ]));
        }
    }
}