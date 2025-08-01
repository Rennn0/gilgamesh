using System.Runtime.Intrinsics.Arm;
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

        [TestMethod]
        public void Ranking()
        {
            SearchEngine searchEngine = new SearchEngine();
            int[] ranking = searchEngine.Ranking([1, 4, 6, 9]);
            CollectionAssert.AreEquivalent(new int[] { 216, 54, 36, 24 }, ranking);
        }

        [TestMethod]
        public void Ptree()
        {
            Solution.PrefixTree<string> prefixTree = new Solution.PrefixTree<string>();
            prefixTree.Insert("luka", "{some json crack}");
            prefixTree.Insert("danelia", "{some json crack AGAIN}");
            prefixTree.Insert("lukito", "{some AGAIN}");

            prefixTree.Search("luka", out string? r1);
            prefixTree.Search("lukito", out string? r2);
            prefixTree.Search("danelia", out string? r3);
            prefixTree.Search("lukaaaaa", out string? r4);


            Assert.AreEqual("{some json crack}", r1);
            Assert.AreEqual("{some AGAIN}", r2);
            Assert.AreEqual("{some json crack AGAIN}", r3);
            Assert.AreEqual(null, r4);
        }

        [TestMethod]
        public void Coins()
        {
            SearchEngine searchEngine = new SearchEngine();
            int c1 = searchEngine.CoinChange([1, 3, 5], 11);
            int c2 = searchEngine.CoinChange([2, 4, 6], 11);
            int c3 = searchEngine.CoinChange([2, 3, 4, 6, 8], 8);
            int c4 = searchEngine.CoinChange([2], 4);
            int c5 = searchEngine.CoinChange([5], 3);
            Assert.AreEqual(3, c1);
            Assert.AreEqual(-1, c2);
            Assert.AreEqual(1, c3);
            Assert.AreEqual(2, c4);
            Assert.AreEqual(-1, c5);
        }

        [TestMethod]
        public void Coins2()
        {
            SearchEngine searchEngine = new SearchEngine();
            int c1 = searchEngine.CoinChange2([1, 2, 5], 5);
            int c2 = searchEngine.CoinChange2([4], 6);
            int c3 = searchEngine.CoinChange2([5], 5);
            int c4 = searchEngine.CoinChange2([1, 2, 5], 0);
            int c5 = searchEngine.CoinChange2([2, 3, 6, 7], 7);
            Assert.AreEqual(4, c1);
            Assert.AreEqual(0, c2);
            Assert.AreEqual(1, c3);
            Assert.AreEqual(0, c4);
            Assert.AreEqual(2, c5);
        }
    }
}