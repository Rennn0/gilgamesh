using System.Collections.ObjectModel;

namespace __TESTS__.Netflix;

[TestClass]
public class GroupSimilarTitles
{
    [TestMethod]
    public void Test()
    {
        EducativeIo.Projects.Netflix.Netflix netflix = new EducativeIo.Projects.Netflix.Netflix();

        netflix.AddRange(["abbcc", "dule", "duel", "deul", "speed", "spede", "cars"]);
        ReadOnlyCollection<string> result = netflix.Search("duel").ToArray().AsReadOnly();

        Assert.AreEqual("duel", result.ElementAt(0));
        Assert.AreEqual(3, result.Count);
    }
}
