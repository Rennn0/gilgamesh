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

        [TestMethod]
        public void NumProvinces()
        {
            Assert.AreEqual(2, Solution.FindProvincesNum(
                [
                    [1, 1, 0],
                    [1, 1, 0],
                    [0, 0, 1]
                ]));
            Assert.AreEqual(2, Solution.FindProvincesNum(
                [
                    [1, 0, 0, 1, 0, 0, 1, 1, 0],
                    [0, 1, 0, 0, 0, 0, 0, 1, 0],
                    [0, 0, 1, 1, 1, 1, 0, 1, 0],
                    [1, 0, 1, 1, 0, 0, 0, 0, 0],
                    [0, 0, 1, 0, 1, 0, 0, 0, 0],
                    [0, 0, 1, 0, 0, 1, 0, 0, 0],
                    [1, 0, 0, 0, 0, 0, 1, 0, 0],
                    [1, 1, 1, 0, 0, 0, 0, 1, 0],
                    [0, 0, 0, 0, 0, 0, 0, 0, 1]
                ]));
            Assert.AreEqual(3, Solution.FindProvincesNum(
                [
                    [1, 0, 0],
                    [0, 1, 0],
                    [0, 0, 1]
                ]));
            Assert.AreEqual(1, Solution.FindProvincesNum(
                [
                    [1, 1, 0, 1],
                    [1, 1, 0, 1],
                    [0, 0, 1, 1],
                    [1, 1, 1, 1]
                ]));
        }

        [TestMethod]
        public void CountConnectedComp()
        {
            Assert.AreEqual(2, Solution.CountConnectedComp(
                [
                    [0, 1],
                    [1, 2],
                    [3, 4]
                ], 5));
            Assert.AreEqual(1, Solution.CountConnectedComp(
                [
                    [0, 1],
                    [1, 2],
                    [2, 3],
                    [3, 4],
                ], 5));
            Assert.AreEqual(2, Solution.CountConnectedComp(
                [
                    [0, 3],
                    [3, 4],
                    [3, 5],
                    [1, 2],
                    [2, 7],
                    [6, 7]
                ], 8));
        }
    }
}