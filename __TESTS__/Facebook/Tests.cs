using EducativeIo.Projects.Facebook;

namespace __TESTS__.Facebook
{
    [TestClass]
    public class FacebookTests
    {
        [TestMethod]
        public void NumIslands()
        {
            Assert.AreEqual(1, Solution.NumIslands([
                ["1", "1", "0", "0", "0"],
                ["1", "1", "0", "0", "0"],
                ["0", "0", "1", "0", "0"],
                ["0", "0", "0", "1", "1"]
            ]));

            Assert.AreEqual(2, Solution.NumIslands([
                ["1", "0", "1", "0", "1"],
                ["0", "1", "0", "1", "0"],
                ["0", "0", "1", "0", "0"],
                ["1", "0", "0", "1", "1"]
            ]));
        }
    }
}