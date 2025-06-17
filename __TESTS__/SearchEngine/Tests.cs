using EducativeIo.Projects.SE;

namespace __TESTS__.SE
{
    [TestClass]
    public class SearchEngineTests
    {
        [TestMethod]
        public void Setup()
        {
            SearchEngine searchEngine = new SearchEngine();
            string[] strings = ["the", "a", "there", "answer", "any", "by", "bye", "their", "abc"];
            foreach (string s in strings) searchEngine.Insert(s);

            Assert.IsTrue(searchEngine.ContainsWord("any"));
            Assert.IsTrue(searchEngine.ContainsWord("answer"));
            Assert.IsFalse(searchEngine.ContainsWord("anything"));
            Assert.IsFalse(searchEngine.ContainsWord("ab"));
            Assert.IsTrue(searchEngine.StartsWith("ans"));
            Assert.IsTrue(searchEngine.StartsWith("t"));
            Assert.IsFalse(searchEngine.StartsWith("anyone"));
            Assert.IsFalse(searchEngine.StartsWith("abcd"));
        }
    }
}