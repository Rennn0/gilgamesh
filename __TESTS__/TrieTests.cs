using EducativeIo.Trie;

namespace __TESTS__;

[TestClass]
public class TrieTests
{
    [TestMethod]
    public void Basic()
    {
        Trie trie = new Trie();
        trie.InsertNode("hello");
        trie.InsertNode("world");

        Assert.IsTrue(trie.SearchNode("hello"));
        Assert.IsTrue(trie.SearchNode("world"));
        Assert.IsFalse(trie.SearchNode("worl"));
    }
}
