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
        string sym = ht.FindSymmetric([[3, 4], [3, 2], [5, 9], [4, 3], [9, 5]], 5);
    }
}
