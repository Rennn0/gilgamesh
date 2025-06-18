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
            foreach (string s in strings)
            {
                searchEngine.Insert(s);
            }

            Assert.IsTrue(searchEngine.ContainsWord("any"));
            Assert.IsTrue(searchEngine.ContainsWord("answer"));
            Assert.IsFalse(searchEngine.ContainsWord("anything"));
            Assert.IsFalse(searchEngine.ContainsWord("ab"));
            Assert.IsTrue(searchEngine.StartsWith("ans"));
            Assert.IsTrue(searchEngine.StartsWith("t"));
            Assert.IsFalse(searchEngine.StartsWith("anyone"));
            Assert.IsFalse(searchEngine.StartsWith("abcd"));
        }


        [TestMethod]
        public void Autocomplete()
        {
            // string[] sentences = ["beautiful", "best quotes", "best friend", "best birthday wishes", "instagram", "internet"];
            // int[] times = [30, 14, 21, 10, 10, 15];

            string[] sentences = ["abc", "aad", "adef"];
            int[] times = [30, 14, 40];

            SearchEngine searchEngine = new SearchEngine();
            searchEngine.StartAutoComplete(sentences, times);

            List<string> b = searchEngine.Complete('b');
            Assert.AreEqual(0, b.Count);
            searchEngine.ResetSearch();
            List<string> a = searchEngine.Complete('a');
            Assert.AreEqual(2, a.Count);
            List<string> aa = searchEngine.Complete('a');
            Assert.AreEqual(1, aa.Count);
        }

        [TestMethod]
        public void BrokenQuery()
        {
            SearchEngine searchEngine = new SearchEngine();
            Assert.IsTrue(searchEngine.BrokenQuery("vegancookbook", ["i", "cream", "cook", "scream", "ice", "cat", "book", "icecream", "vegan"]));
            Assert.IsTrue(searchEngine.BrokenQuery("catice", ["i", "cream", "cook", "scream", "ice", "cat", "book", "icecream", "vegan"]));
            Assert.IsFalse(searchEngine.BrokenQuery("iscreamcooktail", ["i", "cream", "cook", "scream", "ice", "cat", "book", "icecream", "vegan"]));
        }

        [TestMethod]
        public void Query()
        {
            SearchEngine searchEngine = new SearchEngine();
            string query = "vegancookbook";
            string[] dict = ["an", "book", "car", "cat", "cook", "cookbook", "crash",
                "cream", "high", "highway", "i", "ice", "icecream", "low",
                "scream", "veg", "vegan", "way"];

            List<string> q1 = searchEngine.BreakQuery(query, dict);
            query = "icecreamvegan";
            List<string> q2 = searchEngine.BreakQuery(query, dict);
        }
    }
}