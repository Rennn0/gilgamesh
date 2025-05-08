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
        trie.InsertNode("he");
        trie.InsertNode("world");

        Assert.IsTrue(trie.SearchNode("hello"));
        Assert.IsTrue(trie.SearchNode("world"));
        Assert.IsFalse(trie.SearchNode("worl"));
    }

    [TestMethod]
    public void Delete()
    {
        Trie trie = new Trie();
        trie.InsertNode("hello");
        trie.InsertNode("world");
        trie.InsertNode("the");
        trie.InsertNode("their");

        trie.DeleteNode("their");

        Assert.IsFalse(trie.SearchNode("their"));
        Assert.IsTrue(trie.SearchNode("WORLD"));
    }

    [TestMethod]
    public void TotalWords()
    {
        Trie trie = new Trie();
        trie.InsertNode("ab");
        trie.InsertNode("abcd");
        trie.InsertNode("abd");
        trie.InsertNode("aca");
        trie.InsertNode("acad");
        trie.InsertNode("da");
        trie.InsertNode("dab");
        trie.InsertNode("dc");
        trie.InsertNode("dcba");

        Assert.AreEqual(9, trie.TotalWords());
    }

    [TestMethod]
    public void FindWords()
    {
        Trie trie = new Trie();
        trie.InsertNode("the");
        trie.InsertNode("a");
        trie.InsertNode("there");
        trie.InsertNode("answer");
        trie.InsertNode("any");
        trie.InsertNode("by");
        trie.InsertNode("bye");
        trie.InsertNode("their");
        trie.InsertNode("abc");

        List<string> words = trie.FindWords();
        Assert.AreEqual(9, words.Count);
    }
}
