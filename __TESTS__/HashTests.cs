using EducativeIo.Hash;

namespace __TESTS__;

[TestClass]
public class HashTests
{
    [TestMethod]
    public void Basic()
    {
        HashTable hashTable = new HashTable();
        hashTable.Insert("a", 1);
        hashTable.Insert("b", 2);
        hashTable.Insert("c", 3);
        hashTable.Insert("d", 4);
        hashTable.Insert("b", 5);
        hashTable.Insert("f", 6);

        int? value = hashTable.Search("b");
        Assert.AreEqual(5, value);
        hashTable.Delete("b");
        value = hashTable.Search("b");
        Assert.AreEqual(2, value);
    }

    [TestMethod]
    public void Symetric()
    {
        HashTable ht = new HashTable();
        string sym = ht.FindSymmetric(
            [
                [3, 4],
                [3, 2],
                [5, 9],
                [4, 3],
                [9, 5],
            ],
            5
        );
    }

    [TestMethod]
    public void Path()
    {
        Dictionary<string, string> map = new Dictionary<string, string>
        {
            { "NewYork", "Chicago" },
            { "Boston", "Texas" },
            { "Missouri", "NewYork" },
            { "Texas", "Missouri" },
        };
        HashTable ht = new HashTable();
        string path = ht.TracePath(map);
    }

    [TestMethod]
    public void Pairs()
    {
        var pair = new HashTable().FindPair([3, 4, 7, 1, 12, 9]);
    }

    [TestMethod]
    public void SubZero()
    {
        bool has = new HashTable().FindSubZero([6, 4, -7, 3, 12, 9]);
    }

    [TestMethod]
    public void Sum()
    {
        var sum = new HashTable().FindSum([1, 21, 3, 14, 5, 60, 7, 6], 81);
    }
}
